using ConstructionEstimator.Core.Models;
using ConstructionEstimator.Core.Services;

namespace ConstructionEstimator.Infrastructure.Services;

public class MaterialPriceUpdateService : IMaterialPriceUpdateService
{
    private readonly IProvinceConfigService _provinceConfigService;
    private readonly IPriceHistoryService _priceHistoryService;
    private readonly INotificationService _notificationService;
    private readonly List<MaterialPrice> _currentPrices = new();
    private static int _nextId = 1;

    public event EventHandler<MaterialPriceUpdatedEventArgs>? PriceUpdated;

    public MaterialPriceUpdateService(
        IProvinceConfigService provinceConfigService,
        IPriceHistoryService priceHistoryService,
        INotificationService notificationService)
    {
        _provinceConfigService = provinceConfigService;
        _priceHistoryService = priceHistoryService;
        _notificationService = notificationService;
        
        // Initialize with mock data
        InitializeMockData();
    }

    public async Task<IEnumerable<MaterialPrice>> GetMaterialPricesAsync(string? province = null)
    {
        await Task.Delay(200); // Simulate network delay
        
        if (string.IsNullOrEmpty(province))
        {
            return _currentPrices.OrderBy(p => p.Province).ThenBy(p => p.MaterialName).ToList();
        }
        
        return _currentPrices
            .Where(p => p.Province.Equals(province, StringComparison.OrdinalIgnoreCase))
            .OrderBy(p => p.MaterialName)
            .ToList();
    }

    public async Task<bool> UpdateMaterialPricesAsync(string province)
    {
        try
        {
            await Task.Delay(500); // Simulate crawling/API call delay
            
            if (!_provinceConfigService.IsProvinceSupported(province))
            {
                await _notificationService.SendSystemNotificationAsync(
                    $"Tỉnh {province} không được hỗ trợ", "Error");
                return false;
            }

            // Simulate price updates for the province
            var provincePrices = _currentPrices.Where(p => p.Province.Equals(province, StringComparison.OrdinalIgnoreCase)).ToList();
            var random = new Random();
            
            foreach (var price in provincePrices)
            {
                var oldPrice = new MaterialPrice
                {
                    Id = price.Id,
                    MaterialName = price.MaterialName,
                    Province = price.Province,
                    Price = price.Price,
                    Unit = price.Unit,
                    EffectiveDate = price.EffectiveDate,
                    LastUpdated = price.LastUpdated,
                    Source = price.Source,
                    Notes = price.Notes
                };

                // Add price history before updating
                await _priceHistoryService.AddPriceHistoryAsync(oldPrice);

                // Simulate price change (±5%)
                var changePercent = (random.NextDouble() - 0.5) * 0.1; // -5% to +5%
                var newPrice = Math.Max(1000, price.Price * (1 + (decimal)changePercent));
                
                price.Price = Math.Round(newPrice, 0);
                price.LastUpdated = DateTime.Now;
                price.Notes = $"Updated via mock service at {DateTime.Now:HH:mm:ss}";

                // Notify about significant changes (>2%)
                if (Math.Abs(changePercent) > 0.02)
                {
                    await _notificationService.SendPriceChangeNotificationAsync(oldPrice, price);
                }

                OnPriceUpdated(new MaterialPriceUpdatedEventArgs(price, "Updated"));
            }

            await _notificationService.SendSystemNotificationAsync(
                $"Cập nhật giá vật liệu cho {province} thành công", "Update");
            
            return true;
        }
        catch (Exception ex)
        {
            await _notificationService.SendSystemNotificationAsync(
                $"Lỗi cập nhật giá cho {province}: {ex.Message}", "Error");
            return false;
        }
    }

    public async Task<IEnumerable<MaterialPrice>> RefreshAllPricesAsync()
    {
        await _notificationService.SendSystemNotificationAsync("Bắt đầu cập nhật toàn bộ giá vật liệu", "Update");
        
        var provinces = _provinceConfigService.GetProvinces().Take(5); // Limit for demo
        var tasks = provinces.Select(province => UpdateMaterialPricesAsync(province));
        
        await Task.WhenAll(tasks);
        
        await _notificationService.SendSystemNotificationAsync("Hoàn thành cập nhật toàn bộ giá vật liệu", "Update");
        
        return await GetMaterialPricesAsync();
    }

    protected virtual void OnPriceUpdated(MaterialPriceUpdatedEventArgs e)
    {
        PriceUpdated?.Invoke(this, e);
    }

    private void InitializeMockData()
    {
        var materials = new[]
        {
            new { Name = "Xi măng PCB40", Unit = "tấn" },
            new { Name = "Thép CB240-T", Unit = "tấn" },
            new { Name = "Thép CB300-V", Unit = "tấn" },
            new { Name = "Gạch block", Unit = "viên" },
            new { Name = "Cát xây dựng", Unit = "m³" },
            new { Name = "Đá 1x2", Unit = "m³" },
            new { Name = "Đá 4x6", Unit = "m³" },
            new { Name = "Gỗ thông", Unit = "m³" },
            new { Name = "Gỗ cao su", Unit = "m³" },
            new { Name = "Đinh 2.5", Unit = "kg" }
        };

        var provinces = new[] { "Hà Nội", "Hồ Chí Minh", "Đà Nẵng", "Hải Phòng", "Cần Thơ" };
        var basePrices = new decimal[] { 2800000, 18500000, 19200000, 3200, 280000, 420000, 380000, 8500000, 7200000, 32000 };
        var random = new Random(42); // Fixed seed for consistent mock data

        foreach (var province in provinces)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                var material = materials[i];
                var basePrice = basePrices[i];
                
                // Add regional variation (±10%)
                var variation = (decimal)(random.NextDouble() - 0.5) * 0.2m;
                var regionalPrice = Math.Round(basePrice * (1 + variation), 0);

                _currentPrices.Add(new MaterialPrice
                {
                    Id = _nextId++,
                    MaterialName = material.Name,
                    Province = province,
                    Price = regionalPrice,
                    Unit = material.Unit,
                    EffectiveDate = DateTime.Now.AddDays(-random.Next(1, 30)),
                    LastUpdated = DateTime.Now.AddMinutes(-random.Next(1, 1440)),
                    Source = $"Mock API {province}",
                    Notes = "Dữ liệu mô phỏng"
                });
            }
        }
    }
}