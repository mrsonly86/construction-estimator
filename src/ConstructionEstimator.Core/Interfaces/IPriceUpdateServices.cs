using ConstructionEstimator.Core.Entities;

namespace ConstructionEstimator.Core.Interfaces;

public interface IMaterialPriceUpdateService
{
    Task<bool> UpdatePricesForProvinceAsync(string provinceCode);
    Task<bool> UpdateAllProvincesAsync();
    Task<bool> UpdateProvinceAsync(string provinceCode, bool forceUpdate = false);
    Task<PriceUpdateResult> GetLastUpdateStatusAsync(string provinceCode);
    Task<IEnumerable<PriceUpdateResult>> GetUpdateHistoryAsync(string provinceCode, int days = 30);
}

public interface IProvinceConfigService
{
    Task<ProvinceConfig?> GetConfigAsync(string provinceCode);
    Task<IEnumerable<ProvinceConfig>> GetAllConfigsAsync();
    Task<bool> UpdateConfigAsync(ProvinceConfig config);
    Task<bool> TestConnectionAsync(string provinceCode);
    Task<IEnumerable<Province>> GetSupportedProvincesAsync();
}

public interface IPriceHistoryService
{
    Task<bool> LogPriceChangeAsync(int materialId, string provinceCode, decimal oldPrice, decimal newPrice, string source);
    Task<IEnumerable<PriceHistory>> GetPriceHistoryAsync(int materialId, string provinceCode, int days = 90);
    Task<IEnumerable<PriceHistory>> GetSignificantChangesAsync(decimal thresholdPercentage = 10.0m, int days = 30);
    Task<decimal> GetAveragePriceAsync(int materialId, string provinceCode, int days = 30);
    Task<PriceTrend> GetPriceTrendAsync(int materialId, string provinceCode, int days = 90);
}

public interface INotificationService
{
    Task SendPriceAlertAsync(PriceChangeNotification notification);
    Task<bool> ShouldNotifyAsync(PriceHistory priceChange);
    Task<IEnumerable<NotificationSetting>> GetUserNotificationSettingsAsync(int userId);
    Task<bool> UpdateNotificationSettingsAsync(int userId, NotificationSetting settings);
}

// Supporting classes
public class PriceUpdateResult
{
    public string ProvinceCode { get; set; } = string.Empty;
    public DateTime UpdateTime { get; set; }
    public bool Success { get; set; }
    public int MaterialsUpdated { get; set; }
    public int MaterialsAdded { get; set; }
    public string? ErrorMessage { get; set; }
    public TimeSpan Duration { get; set; }
}

public class PriceTrend
{
    public TrendDirection Direction { get; set; }
    public decimal ChangePercentage { get; set; }
    public decimal AveragePrice { get; set; }
    public int DataPoints { get; set; }
}

public enum TrendDirection
{
    Rising = 1,
    Falling = 2,
    Stable = 3,
    Volatile = 4
}

public class PriceChangeNotification
{
    public string MaterialName { get; set; } = string.Empty;
    public string ProvinceName { get; set; } = string.Empty;
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public decimal ChangePercentage { get; set; }
    public DateTime ChangeDate { get; set; }
    public NotificationSeverity Severity { get; set; }
}

public enum NotificationSeverity
{
    Info = 1,
    Warning = 2,
    Critical = 3
}

public class NotificationSetting
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool EmailEnabled { get; set; } = true;
    public bool PriceAlertEnabled { get; set; } = true;
    public decimal PriceChangeThreshold { get; set; } = 10.0m; // Percentage
    public bool DailyDigestEnabled { get; set; } = false;
    public string? EmailAddress { get; set; }
}