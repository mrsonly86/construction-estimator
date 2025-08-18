using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ConstructionEstimator.Core.Services;

public class MaterialPriceUpdateService : IMaterialPriceUpdateService
{
    private readonly IMaterialPriceRepository _materialPriceRepository;
    private readonly IDataSourceService _dataSourceService;
    private readonly IPriceHistoryService _priceHistoryService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<MaterialPriceUpdateService> _logger;

    public MaterialPriceUpdateService(
        IMaterialPriceRepository materialPriceRepository,
        IDataSourceService dataSourceService,
        IPriceHistoryService priceHistoryService,
        INotificationService notificationService,
        ILogger<MaterialPriceUpdateService> logger)
    {
        _materialPriceRepository = materialPriceRepository;
        _dataSourceService = dataSourceService;
        _priceHistoryService = priceHistoryService;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<bool> UpdatePricesForProvinceAsync(int provinceId)
    {
        try
        {
            _logger.LogInformation("Starting price update for province {ProvinceId}", provinceId);
            
            var dataSources = await _dataSourceService.GetDataSourcesByProvinceAsync(provinceId);
            var activeDataSources = dataSources.Where(ds => ds.IsActive);
            
            if (!activeDataSources.Any())
            {
                _logger.LogWarning("No active data sources found for province {ProvinceId}", provinceId);
                return false;
            }

            bool overallSuccess = true;
            int successCount = 0;
            int totalCount = activeDataSources.Count();

            foreach (var dataSource in activeDataSources)
            {
                try
                {
                    var success = await UpdatePricesFromDataSourceAsync(dataSource);
                    if (success)
                    {
                        successCount++;
                        
                        // Update the data source scan information
                        dataSource.LastScanDate = DateTime.Now;
                        dataSource.NextScanDate = DateTime.Now.AddDays(dataSource.UpdateFrequencyDays);
                        await _dataSourceService.UpdateDataSourceAsync(dataSource);
                    }
                    else
                    {
                        overallSuccess = false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating prices from data source {DataSourceId} for province {ProvinceId}", 
                        dataSource.Id, provinceId);
                    overallSuccess = false;
                }
            }

            _logger.LogInformation("Price update completed for province {ProvinceId}: {SuccessCount}/{TotalCount} sources successful", 
                provinceId, successCount, totalCount);

            return overallSuccess;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during price update for province {ProvinceId}", provinceId);
            return false;
        }
    }

    public async Task<bool> UpdatePricesForMaterialAsync(int materialId)
    {
        try
        {
            _logger.LogInformation("Starting price update for material {MaterialId}", materialId);
            
            var materialPrices = await _materialPriceRepository.GetMaterialPricesForMaterialAsync(materialId);
            var provinceIds = materialPrices.Select(mp => mp.ProvinceId).Distinct();
            
            bool overallSuccess = true;
            int successCount = 0;
            int totalCount = provinceIds.Count();

            foreach (var provinceId in provinceIds)
            {
                try
                {
                    var success = await UpdatePricesForProvinceAsync(provinceId);
                    if (success)
                    {
                        successCount++;
                    }
                    else
                    {
                        overallSuccess = false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating prices for material {MaterialId} in province {ProvinceId}", 
                        materialId, provinceId);
                    overallSuccess = false;
                }
            }

            _logger.LogInformation("Price update completed for material {MaterialId}: {SuccessCount}/{TotalCount} provinces successful", 
                materialId, successCount, totalCount);

            return overallSuccess;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during price update for material {MaterialId}", materialId);
            return false;
        }
    }

    public async Task<bool> UpdateAllPricesAsync()
    {
        try
        {
            _logger.LogInformation("Starting comprehensive price update for all active data sources");
            
            var dataSourcesDue = await _dataSourceService.GetDataSourcesDueForUpdateAsync();
            
            if (!dataSourcesDue.Any())
            {
                _logger.LogInformation("No data sources are due for update");
                return true;
            }

            bool overallSuccess = true;
            int successCount = 0;
            int totalCount = dataSourcesDue.Count();

            foreach (var dataSource in dataSourcesDue)
            {
                try
                {
                    var success = await UpdatePricesFromDataSourceAsync(dataSource);
                    if (success)
                    {
                        successCount++;
                        
                        // Update the data source scan information
                        dataSource.LastScanDate = DateTime.Now;
                        dataSource.NextScanDate = DateTime.Now.AddDays(dataSource.UpdateFrequencyDays);
                        await _dataSourceService.UpdateDataSourceAsync(dataSource);
                    }
                    else
                    {
                        overallSuccess = false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating prices from data source {DataSourceId}", dataSource.Id);
                    overallSuccess = false;
                }
            }

            _logger.LogInformation("Comprehensive price update completed: {SuccessCount}/{TotalCount} data sources successful", 
                successCount, totalCount);

            return overallSuccess;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during comprehensive price update");
            return false;
        }
    }

    public async Task<IEnumerable<MaterialPrice>> GetMaterialPricesByProvinceAsync(int provinceId)
    {
        try
        {
            _logger.LogInformation("Retrieving material prices for province {ProvinceId}", provinceId);
            return await _materialPriceRepository.GetMaterialPricesByProvinceAsync(provinceId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving material prices for province {ProvinceId}", provinceId);
            throw;
        }
    }

    public async Task<IEnumerable<MaterialPrice>> GetMaterialPricesForMaterialAsync(int materialId, int? provinceId = null)
    {
        try
        {
            _logger.LogInformation("Retrieving prices for material {MaterialId}, province {ProvinceId}", 
                materialId, provinceId);
            return await _materialPriceRepository.GetMaterialPricesForMaterialAsync(materialId, provinceId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving prices for material {MaterialId}, province {ProvinceId}", 
                materialId, provinceId);
            throw;
        }
    }

    public async Task<MaterialPrice?> GetCurrentPriceAsync(int materialId, int provinceId)
    {
        try
        {
            _logger.LogInformation("Retrieving current price for material {MaterialId} in province {ProvinceId}", 
                materialId, provinceId);
            return await _materialPriceRepository.GetCurrentPriceAsync(materialId, provinceId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving current price for material {MaterialId} in province {ProvinceId}", 
                materialId, provinceId);
            throw;
        }
    }

    private async Task<bool> UpdatePricesFromDataSourceAsync(DataSource dataSource)
    {
        try
        {
            _logger.LogInformation("Updating prices from data source: {DataSourceName} ({DataSourceType})", 
                dataSource.Name, dataSource.SourceType);

            // Parse scan configuration
            var config = ParseScanConfiguration(dataSource.ScanConfiguration);
            
            // Fetch data based on source type
            var priceData = await FetchPriceDataAsync(dataSource, config);
            
            if (priceData == null || !priceData.Any())
            {
                _logger.LogWarning("No price data retrieved from data source {DataSourceId}", dataSource.Id);
                return false;
            }

            int updatedCount = 0;
            foreach (var priceInfo in priceData)
            {
                try
                {
                    var updated = await UpdateMaterialPriceAsync(priceInfo, dataSource);
                    if (updated) updatedCount++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating individual price from data source {DataSourceId}", dataSource.Id);
                }
            }

            _logger.LogInformation("Updated {UpdatedCount} prices from data source {DataSourceName}", 
                updatedCount, dataSource.Name);
            
            return updatedCount > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating prices from data source {DataSourceId}", dataSource.Id);
            return false;
        }
    }

    private ScanConfiguration ParseScanConfiguration(string configJson)
    {
        try
        {
            if (string.IsNullOrEmpty(configJson))
                return new ScanConfiguration();

            return JsonSerializer.Deserialize<ScanConfiguration>(configJson) ?? new ScanConfiguration();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error parsing scan configuration, using defaults");
            return new ScanConfiguration();
        }
    }

    private async Task<IEnumerable<PriceDataInfo>> FetchPriceDataAsync(DataSource dataSource, ScanConfiguration config)
    {
        // This is a simplified implementation
        // In a real application, you would implement different fetchers for different source types
        
        _logger.LogInformation("Fetching price data from {SourceType}: {SourceUrl}", 
            dataSource.SourceType, dataSource.SourceUrl);

        try
        {
            switch (dataSource.SourceType.ToLower())
            {
                case "url":
                    return await FetchFromUrlAsync(dataSource.SourceUrl, config);
                case "excel":
                    return await FetchFromExcelAsync(dataSource.SourceUrl, config);
                case "pdf":
                    return await FetchFromPdfAsync(dataSource.SourceUrl, config);
                case "api":
                    return await FetchFromApiAsync(dataSource.SourceUrl, config);
                default:
                    _logger.LogWarning("Unsupported source type: {SourceType}", dataSource.SourceType);
                    return Enumerable.Empty<PriceDataInfo>();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data from {SourceType}: {SourceUrl}", 
                dataSource.SourceType, dataSource.SourceUrl);
            return Enumerable.Empty<PriceDataInfo>();
        }
    }

    private async Task<IEnumerable<PriceDataInfo>> FetchFromUrlAsync(string url, ScanConfiguration config)
    {
        // TODO: Implement web scraping logic
        _logger.LogInformation("Web scraping from URL: {Url}", url);
        
        // Mock implementation - returns sample data
        await Task.Delay(1000); // Simulate network delay
        
        return new List<PriceDataInfo>
        {
            new PriceDataInfo { MaterialCode = "XM001", UnitPrice = 110000, Supplier = "Web Source" }
        };
    }

    private async Task<IEnumerable<PriceDataInfo>> FetchFromExcelAsync(string filePath, ScanConfiguration config)
    {
        // TODO: Implement Excel reading logic using EPPlus or similar
        _logger.LogInformation("Reading Excel file: {FilePath}", filePath);
        
        await Task.Delay(500); // Simulate file processing
        
        return new List<PriceDataInfo>
        {
            new PriceDataInfo { MaterialCode = "XM001", UnitPrice = 105000, Supplier = "Excel Source" }
        };
    }

    private async Task<IEnumerable<PriceDataInfo>> FetchFromPdfAsync(string filePath, ScanConfiguration config)
    {
        // TODO: Implement PDF reading logic using iTextSharp or similar
        _logger.LogInformation("Reading PDF file: {FilePath}", filePath);
        
        await Task.Delay(1500); // Simulate PDF processing
        
        return new List<PriceDataInfo>
        {
            new PriceDataInfo { MaterialCode = "XM001", UnitPrice = 108000, Supplier = "PDF Source" }
        };
    }

    private async Task<IEnumerable<PriceDataInfo>> FetchFromApiAsync(string apiUrl, ScanConfiguration config)
    {
        // TODO: Implement API fetching logic
        _logger.LogInformation("Fetching from API: {ApiUrl}", apiUrl);
        
        await Task.Delay(800); // Simulate API call
        
        return new List<PriceDataInfo>
        {
            new PriceDataInfo { MaterialCode = "XM001", UnitPrice = 107000, Supplier = "API Source" }
        };
    }

    private async Task<bool> UpdateMaterialPriceAsync(PriceDataInfo priceInfo, DataSource dataSource)
    {
        try
        {
            // Find existing material price record
            var existingPrice = await _materialPriceRepository.GetCurrentPriceAsync(
                priceInfo.MaterialId ?? 0, dataSource.ProvinceId);

            if (existingPrice != null && Math.Abs(existingPrice.UnitPrice - priceInfo.UnitPrice) < 0.01m)
            {
                // Price hasn't changed significantly
                return false;
            }

            if (existingPrice != null)
            {
                // Record price change history
                await _priceHistoryService.RecordPriceChangeAsync(
                    existingPrice.Id, existingPrice.UnitPrice, priceInfo.UnitPrice, dataSource.Name);

                // Calculate percentage change for alerts
                var changePercentage = existingPrice.UnitPrice != 0 
                    ? ((priceInfo.UnitPrice - existingPrice.UnitPrice) / existingPrice.UnitPrice) * 100 
                    : 0;

                // Check and trigger alerts if necessary
                await _notificationService.CheckAndTriggerAlertsAsync(
                    existingPrice.MaterialId, dataSource.ProvinceId, changePercentage);

                // Update existing price
                existingPrice.UnitPrice = priceInfo.UnitPrice;
                existingPrice.Supplier = priceInfo.Supplier;
                existingPrice.LastUpdated = DateTime.Now;
                existingPrice.DataSourceUrl = dataSource.SourceUrl;
                
                await _materialPriceRepository.UpdateAsync(existingPrice);
            }
            else
            {
                // Create new price record
                var newPrice = new MaterialPrice
                {
                    MaterialId = priceInfo.MaterialId ?? 0,
                    ProvinceId = dataSource.ProvinceId,
                    UnitPrice = priceInfo.UnitPrice,
                    Supplier = priceInfo.Supplier,
                    EffectiveDate = DateTime.Now,
                    DataSourceUrl = dataSource.SourceUrl,
                    IsVerified = false,
                    CreatedDate = DateTime.Now,
                    LastUpdated = DateTime.Now
                };

                await _materialPriceRepository.AddAsync(newPrice);
            }

            await _materialPriceRepository.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating material price for {MaterialCode}", priceInfo.MaterialCode);
            return false;
        }
    }
}

// Helper classes for configuration and data transfer
public class ScanConfiguration
{
    public string MaterialCodeSelector { get; set; } = string.Empty;
    public string PriceSelector { get; set; } = string.Empty;
    public string SupplierSelector { get; set; } = string.Empty;
    public Dictionary<string, string> Headers { get; set; } = new();
    public int DelayMs { get; set; } = 1000;
    public bool UseProxy { get; set; } = false;
}

public class PriceDataInfo
{
    public string MaterialCode { get; set; } = string.Empty;
    public int? MaterialId { get; set; }
    public decimal UnitPrice { get; set; }
    public string Supplier { get; set; } = string.Empty;
    public DateTime? EffectiveDate { get; set; }
}