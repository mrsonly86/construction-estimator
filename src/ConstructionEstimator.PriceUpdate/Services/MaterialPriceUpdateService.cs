using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HtmlAgilityPack;
using ConstructionEstimator.Core.Interfaces;
using ConstructionEstimator.Core.Entities;

namespace ConstructionEstimator.PriceUpdate.Services;

public class MaterialPriceUpdateService : IMaterialPriceUpdateService
{
    private readonly ILogger<MaterialPriceUpdateService> _logger;
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProvinceConfigService _provinceConfigService;
    private readonly IPriceHistoryService _priceHistoryService;
    private readonly PriceUpdateOptions _options;

    public MaterialPriceUpdateService(
        ILogger<MaterialPriceUpdateService> logger,
        HttpClient httpClient,
        IUnitOfWork unitOfWork,
        IProvinceConfigService provinceConfigService,
        IPriceHistoryService priceHistoryService,
        IOptions<PriceUpdateOptions> options)
    {
        _logger = logger;
        _httpClient = httpClient;
        _unitOfWork = unitOfWork;
        _provinceConfigService = provinceConfigService;
        _priceHistoryService = priceHistoryService;
        _options = options.Value;
    }

    public async Task<bool> UpdatePricesForProvinceAsync(string provinceCode)
    {
        try
        {
            _logger.LogInformation("Starting price update for province {ProvinceCode}", provinceCode);

            var config = await _provinceConfigService.GetConfigAsync(provinceCode);
            if (config == null || !config.AutoUpdateEnabled)
            {
                _logger.LogWarning("Province {ProvinceCode} not configured for auto-update", provinceCode);
                return false;
            }

            var result = await ScrapeProvinceWebsiteAsync(config);
            if (result.Success && result.MaterialPrices.Any())
            {
                await ProcessPriceUpdatesAsync(provinceCode, result.MaterialPrices);
                
                // Update last successful update time
                config.LastSuccessfulUpdate = DateTime.UtcNow;
                config.LastUpdateStatus = "Success";
                await _provinceConfigService.UpdateConfigAsync(config);
                
                _logger.LogInformation("Successfully updated {Count} material prices for {ProvinceCode}", 
                    result.MaterialPrices.Count, provinceCode);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating prices for province {ProvinceCode}", provinceCode);
            return false;
        }
    }

    public async Task<bool> UpdateAllProvincesAsync()
    {
        var configs = await _provinceConfigService.GetAllConfigsAsync();
        var tasks = configs.Where(c => c.AutoUpdateEnabled)
                          .Select(c => UpdatePricesForProvinceAsync(c.ProvinceCode));

        var results = await Task.WhenAll(tasks);
        return results.Any(r => r);
    }

    public async Task<bool> UpdateProvinceAsync(string provinceCode, bool forceUpdate = false)
    {
        var config = await _provinceConfigService.GetConfigAsync(provinceCode);
        if (config == null) return false;

        if (!forceUpdate && config.LastSuccessfulUpdate.HasValue)
        {
            var daysSinceLastUpdate = (DateTime.UtcNow - config.LastSuccessfulUpdate.Value).Days;
            if (daysSinceLastUpdate < 1) // Don't update more than once per day
            {
                _logger.LogInformation("Skipping update for {ProvinceCode} - updated recently", provinceCode);
                return true;
            }
        }

        return await UpdatePricesForProvinceAsync(provinceCode);
    }

    public async Task<PriceUpdateResult> GetLastUpdateStatusAsync(string provinceCode)
    {
        var config = await _provinceConfigService.GetConfigAsync(provinceCode);
        if (config == null)
        {
            return new PriceUpdateResult
            {
                ProvinceCode = provinceCode,
                Success = false,
                ErrorMessage = "Province configuration not found"
            };
        }

        return new PriceUpdateResult
        {
            ProvinceCode = provinceCode,
            UpdateTime = config.LastSuccessfulUpdate ?? DateTime.MinValue,
            Success = config.LastUpdateStatus == "Success",
            ErrorMessage = config.LastUpdateStatus != "Success" ? config.LastUpdateStatus : null
        };
    }

    public async Task<IEnumerable<PriceUpdateResult>> GetUpdateHistoryAsync(string provinceCode, int days = 30)
    {
        // This could be implemented with a separate update history table
        // For now, return a single result with the last update
        var lastUpdate = await GetLastUpdateStatusAsync(provinceCode);
        return new[] { lastUpdate };
    }

    private async Task<ScrapeResult> ScrapeProvinceWebsiteAsync(ProvinceConfig config)
    {
        var result = new ScrapeResult();

        try
        {
            if (string.IsNullOrEmpty(config.WebsiteUrl))
            {
                result.ErrorMessage = "Website URL not configured";
                return result;
            }

            var response = await _httpClient.GetStringAsync(config.WebsiteUrl);
            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            // Try to find price table using configured selector
            if (!string.IsNullOrEmpty(config.PriceTableSelector))
            {
                var priceTable = doc.DocumentNode.SelectSingleNode(config.PriceTableSelector);
                if (priceTable != null)
                {
                    result.MaterialPrices = await ParsePriceTableAsync(priceTable, config.ProvinceCode);
                    result.Success = true;
                }
            }

            // If no table found, try to find PDF/Excel download links
            if (!result.Success)
            {
                await TryDownloadPriceFileAsync(doc, config, result);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scraping website for province {ProvinceCode}", config.ProvinceCode);
            result.ErrorMessage = ex.Message;
        }

        return result;
    }

    private async Task<List<ScrapedMaterialPrice>> ParsePriceTableAsync(HtmlNode table, string provinceCode)
    {
        var prices = new List<ScrapedMaterialPrice>();

        try
        {
            var rows = table.SelectNodes(".//tr");
            if (rows == null) return prices;

            foreach (var row in rows.Skip(1)) // Skip header row
            {
                var cells = row.SelectNodes(".//td");
                if (cells?.Count >= 3)
                {
                    var materialName = cells[0].InnerText?.Trim();
                    var unit = cells[1].InnerText?.Trim();
                    var priceText = cells[2].InnerText?.Trim()?.Replace(",", "");

                    if (!string.IsNullOrEmpty(materialName) && 
                        !string.IsNullOrEmpty(priceText) && 
                        decimal.TryParse(priceText, out var price))
                    {
                        prices.Add(new ScrapedMaterialPrice
                        {
                            MaterialName = materialName,
                            Unit = unit ?? "",
                            Price = price,
                            ProvinceCode = provinceCode,
                            EffectiveDate = DateTime.UtcNow
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing price table for province {ProvinceCode}", provinceCode);
        }

        return prices;
    }

    private async Task TryDownloadPriceFileAsync(HtmlDocument doc, ProvinceConfig config, ScrapeResult result)
    {
        // Implementation for downloading and parsing PDF/Excel files would go here
        // This is a complex task that would require additional libraries like iTextSharp for PDF
        // or EPPlus for Excel files
        
        _logger.LogInformation("PDF/Excel file parsing not implemented yet for province {ProvinceCode}", 
            config.ProvinceCode);
    }

    private async Task ProcessPriceUpdatesAsync(string provinceCode, List<ScrapedMaterialPrice> scrapedPrices)
    {
        foreach (var scrapedPrice in scrapedPrices)
        {
            // Find matching material in database
            var materials = await _unitOfWork.Materials.GetAllAsync();
            var material = materials.FirstOrDefault(m => 
                m.Name.Contains(scrapedPrice.MaterialName, StringComparison.OrdinalIgnoreCase) ||
                scrapedPrice.MaterialName.Contains(m.Name, StringComparison.OrdinalIgnoreCase));

            if (material == null)
            {
                _logger.LogWarning("Material not found for scraped price: {MaterialName}", scrapedPrice.MaterialName);
                continue;
            }

            // Get current price
            var currentPrices = await _unitOfWork.MaterialPrices.GetAllAsync();
            var currentPrice = currentPrices.FirstOrDefault(p => 
                p.MaterialId == material.Id && 
                p.ProvinceCode == provinceCode && 
                p.IsActive);

            if (currentPrice != null)
            {
                // Update existing price
                if (Math.Abs(currentPrice.Price - scrapedPrice.Price) > 0.01m)
                {
                    // Log price change
                    await _priceHistoryService.LogPriceChangeAsync(
                        material.Id, 
                        provinceCode, 
                        currentPrice.Price, 
                        scrapedPrice.Price,
                        $"Auto-update from {provinceCode}");

                    currentPrice.Price = scrapedPrice.Price;
                    currentPrice.LastUpdated = DateTime.UtcNow;
                    await _unitOfWork.MaterialPrices.UpdateAsync(currentPrice);
                }
            }
            else
            {
                // Create new price entry
                var newPrice = new MaterialPrice
                {
                    MaterialId = material.Id,
                    ProvinceCode = provinceCode,
                    Price = scrapedPrice.Price,
                    EffectiveDate = scrapedPrice.EffectiveDate,
                    Source = $"Auto-update from {provinceCode}",
                    LastUpdated = DateTime.UtcNow,
                    IsActive = true
                };

                await _unitOfWork.MaterialPrices.AddAsync(newPrice);
            }
        }

        await _unitOfWork.SaveChangesAsync();
    }
}

// Supporting classes
public class PriceUpdateOptions
{
    public int TimeoutSeconds { get; set; } = 30;
    public int RetryCount { get; set; } = 3;
    public bool EnableBackupSources { get; set; } = true;
}

public class ScrapeResult
{
    public bool Success { get; set; }
    public List<ScrapedMaterialPrice> MaterialPrices { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class ScrapedMaterialPrice
{
    public string MaterialName { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ProvinceCode { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
}