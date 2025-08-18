using ConstructionEstimator.Core.Models;
using ConstructionEstimator.Core.Services;

namespace ConstructionEstimator.Infrastructure.Services;

public class PriceHistoryService : IPriceHistoryService
{
    private readonly List<MaterialPrice> _priceHistory = new();

    public async Task<IEnumerable<MaterialPrice>> GetPriceHistoryAsync(string materialName, string? province = null)
    {
        await Task.Delay(50); // Simulate async operation
        
        var query = _priceHistory.Where(p => p.MaterialName.Equals(materialName, StringComparison.OrdinalIgnoreCase));
        
        if (!string.IsNullOrEmpty(province))
        {
            query = query.Where(p => p.Province.Equals(province, StringComparison.OrdinalIgnoreCase));
        }
        
        return query.OrderByDescending(p => p.LastUpdated).ToList();
    }

    public async Task AddPriceHistoryAsync(MaterialPrice price)
    {
        await Task.Delay(50); // Simulate async operation
        
        // Create a copy with a new ID for history
        var historyEntry = new MaterialPrice
        {
            Id = _priceHistory.Count + 1000, // Offset to avoid conflicts
            MaterialName = price.MaterialName,
            Province = price.Province,
            Price = price.Price,
            Unit = price.Unit,
            EffectiveDate = price.EffectiveDate,
            LastUpdated = DateTime.Now,
            Source = price.Source,
            Notes = price.Notes
        };
        
        _priceHistory.Add(historyEntry);
    }

    public async Task<IEnumerable<MaterialPrice>> GetRecentChangesAsync(int days = 30)
    {
        await Task.Delay(50); // Simulate async operation
        
        var cutoffDate = DateTime.Now.AddDays(-days);
        return _priceHistory
            .Where(p => p.LastUpdated >= cutoffDate)
            .OrderByDescending(p => p.LastUpdated)
            .ToList();
    }

    public async Task<decimal?> GetPreviousPriceAsync(string materialName, string province)
    {
        await Task.Delay(50); // Simulate async operation
        
        var previousPrices = _priceHistory
            .Where(p => p.MaterialName.Equals(materialName, StringComparison.OrdinalIgnoreCase) &&
                       p.Province.Equals(province, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(p => p.LastUpdated)
            .ToList();
        
        // Return the second most recent price (skip the current one)
        return previousPrices.Skip(1).FirstOrDefault()?.Price;
    }
}