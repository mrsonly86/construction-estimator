using Microsoft.Extensions.Logging;
using ConstructionEstimator.Core.Interfaces;
using ConstructionEstimator.Core.Entities;

namespace ConstructionEstimator.PriceUpdate.Services;

public class PriceHistoryService : IPriceHistoryService
{
    private readonly ILogger<PriceHistoryService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public PriceHistoryService(
        ILogger<PriceHistoryService> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> LogPriceChangeAsync(int materialId, string provinceCode, decimal oldPrice, decimal newPrice, string source)
    {
        try
        {
            var changePercentage = oldPrice != 0 ? ((newPrice - oldPrice) / oldPrice) * 100 : 0;
            var changeType = newPrice > oldPrice ? PriceChangeType.Increase : 
                           newPrice < oldPrice ? PriceChangeType.Decrease : 
                           PriceChangeType.NoChange;

            var priceHistory = new PriceHistory
            {
                MaterialId = materialId,
                ProvinceCode = provinceCode,
                OldPrice = oldPrice,
                NewPrice = newPrice,
                ChangePercentage = changePercentage,
                ChangeDate = DateTime.UtcNow,
                Source = source,
                ChangeType = changeType
            };

            await _unitOfWork.PriceHistories.AddAsync(priceHistory);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Logged price change for material {MaterialId} in {ProvinceCode}: {OldPrice} -> {NewPrice} ({ChangePercentage:F2}%)",
                materialId, provinceCode, oldPrice, newPrice, changePercentage);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging price change for material {MaterialId}", materialId);
            return false;
        }
    }

    public async Task<IEnumerable<PriceHistory>> GetPriceHistoryAsync(int materialId, string provinceCode, int days = 90)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-days);
        var allHistories = await _unitOfWork.PriceHistories.GetAllAsync();
        
        return allHistories.Where(h => 
            h.MaterialId == materialId && 
            h.ProvinceCode == provinceCode && 
            h.ChangeDate >= cutoffDate)
            .OrderByDescending(h => h.ChangeDate);
    }

    public async Task<IEnumerable<PriceHistory>> GetSignificantChangesAsync(decimal thresholdPercentage = 10.0m, int days = 30)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-days);
        var allHistories = await _unitOfWork.PriceHistories.GetAllAsync();
        
        return allHistories.Where(h => 
            h.ChangeDate >= cutoffDate && 
            Math.Abs(h.ChangePercentage) >= thresholdPercentage)
            .OrderByDescending(h => Math.Abs(h.ChangePercentage));
    }

    public async Task<decimal> GetAveragePriceAsync(int materialId, string provinceCode, int days = 30)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-days);
        var histories = await GetPriceHistoryAsync(materialId, provinceCode, days);
        
        if (!histories.Any())
        {
            // Get current price if no history
            var allPrices = await _unitOfWork.MaterialPrices.GetAllAsync();
            var currentPrice = allPrices.FirstOrDefault(p => 
                p.MaterialId == materialId && 
                p.ProvinceCode == provinceCode && 
                p.IsActive);
            return currentPrice?.Price ?? 0;
        }
        
        return histories.Average(h => h.NewPrice);
    }

    public async Task<PriceTrend> GetPriceTrendAsync(int materialId, string provinceCode, int days = 90)
    {
        var histories = await GetPriceHistoryAsync(materialId, provinceCode, days);
        var historyList = histories.ToList();
        
        if (!historyList.Any())
        {
            return new PriceTrend
            {
                Direction = TrendDirection.Stable,
                ChangePercentage = 0,
                AveragePrice = await GetAveragePriceAsync(materialId, provinceCode, days),
                DataPoints = 0
            };
        }

        var totalChange = historyList.Sum(h => h.ChangePercentage);
        var averagePrice = historyList.Average(h => h.NewPrice);
        var volatility = CalculateVolatility(historyList);

        var direction = volatility > 15 ? TrendDirection.Volatile :
                       totalChange > 5 ? TrendDirection.Rising :
                       totalChange < -5 ? TrendDirection.Falling :
                       TrendDirection.Stable;

        return new PriceTrend
        {
            Direction = direction,
            ChangePercentage = totalChange,
            AveragePrice = averagePrice,
            DataPoints = historyList.Count
        };
    }

    private static decimal CalculateVolatility(List<PriceHistory> histories)
    {
        if (histories.Count < 2) return 0;
        
        var changes = histories.Select(h => Math.Abs(h.ChangePercentage)).ToList();
        var mean = changes.Average();
        var variance = changes.Select(x => (x - mean) * (x - mean)).Average();
        
        return (decimal)Math.Sqrt((double)variance);
    }
}