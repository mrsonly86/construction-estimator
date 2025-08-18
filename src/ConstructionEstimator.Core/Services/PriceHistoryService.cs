using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConstructionEstimator.Core.Services;

public class PriceHistoryService : IPriceHistoryService
{
    private readonly IPriceHistoryRepository _priceHistoryRepository;
    private readonly IMaterialPriceRepository _materialPriceRepository;
    private readonly ILogger<PriceHistoryService> _logger;

    public PriceHistoryService(
        IPriceHistoryRepository priceHistoryRepository, 
        IMaterialPriceRepository materialPriceRepository,
        ILogger<PriceHistoryService> logger)
    {
        _priceHistoryRepository = priceHistoryRepository;
        _materialPriceRepository = materialPriceRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<PriceHistory>> GetPriceHistoryAsync(int materialId, int? provinceId = null)
    {
        try
        {
            _logger.LogInformation("Retrieving price history for material {MaterialId}, province {ProvinceId}", 
                materialId, provinceId);
            return await _priceHistoryRepository.GetPriceHistoryAsync(materialId, provinceId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving price history for material {MaterialId}, province {ProvinceId}", 
                materialId, provinceId);
            throw;
        }
    }

    public async Task<PriceHistory> RecordPriceChangeAsync(int materialPriceId, decimal oldPrice, decimal newPrice, string source)
    {
        try
        {
            _logger.LogInformation("Recording price change for MaterialPrice {MaterialPriceId}: {OldPrice} -> {NewPrice}", 
                materialPriceId, oldPrice, newPrice);

            var changeAmount = newPrice - oldPrice;
            var changePercentage = oldPrice != 0 ? (changeAmount / oldPrice) * 100 : 0;
            
            var changeType = changeAmount > 0 ? "Increase" : 
                            changeAmount < 0 ? "Decrease" : "Stable";

            var priceHistory = new PriceHistory
            {
                MaterialPriceId = materialPriceId,
                OldPrice = oldPrice,
                NewPrice = newPrice,
                ChangeAmount = changeAmount,
                ChangePercentage = changePercentage,
                ChangeType = changeType,
                ChangeDate = DateTime.Now,
                Source = source
            };

            var createdHistory = await _priceHistoryRepository.AddAsync(priceHistory);
            await _priceHistoryRepository.SaveChangesAsync();

            _logger.LogInformation("Successfully recorded price change: {ChangeType} of {ChangePercentage:F2}%", 
                changeType, changePercentage);

            return createdHistory;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording price change for MaterialPrice {MaterialPriceId}", materialPriceId);
            throw;
        }
    }

    public async Task<IEnumerable<PriceHistory>> GetRecentPriceChangesAsync(DateTime fromDate, int? provinceId = null)
    {
        try
        {
            _logger.LogInformation("Retrieving recent price changes from {FromDate} for province {ProvinceId}", 
                fromDate, provinceId);
            return await _priceHistoryRepository.GetRecentPriceChangesAsync(fromDate, provinceId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recent price changes from {FromDate} for province {ProvinceId}", 
                fromDate, provinceId);
            throw;
        }
    }

    public async Task<decimal> GetAveragePriceChangeAsync(int materialId, int days)
    {
        try
        {
            _logger.LogInformation("Calculating average price change for material {MaterialId} over {Days} days", 
                materialId, days);
            return await _priceHistoryRepository.GetAveragePriceChangeAsync(materialId, days);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating average price change for material {MaterialId} over {Days} days", 
                materialId, days);
            throw;
        }
    }

    public async Task<IEnumerable<PriceHistory>> GetSignificantPriceChangesAsync(decimal thresholdPercentage, DateTime fromDate)
    {
        try
        {
            _logger.LogInformation("Retrieving significant price changes (>{ThresholdPercentage}%) from {FromDate}", 
                thresholdPercentage, fromDate);
            return await _priceHistoryRepository.GetSignificantPriceChangesAsync(thresholdPercentage, fromDate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving significant price changes (>{ThresholdPercentage}%) from {FromDate}", 
                thresholdPercentage, fromDate);
            throw;
        }
    }
}