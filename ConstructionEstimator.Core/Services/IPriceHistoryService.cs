using ConstructionEstimator.Core.Models;

namespace ConstructionEstimator.Core.Services;

public interface IPriceHistoryService
{
    Task<IEnumerable<MaterialPrice>> GetPriceHistoryAsync(string materialName, string? province = null);
    Task AddPriceHistoryAsync(MaterialPrice price);
    Task<IEnumerable<MaterialPrice>> GetRecentChangesAsync(int days = 30);
    Task<decimal?> GetPreviousPriceAsync(string materialName, string province);
}