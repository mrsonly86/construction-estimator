namespace ConstructionEstimator.Core.Entities;

public class PriceAlert
{
    public int Id { get; set; }
    public int MaterialId { get; set; }
    public int? ProvinceId { get; set; } // null means all provinces
    public string AlertType { get; set; } = string.Empty; // "PriceIncrease", "PriceDecrease", "SignificantChange"
    public decimal ThresholdPercentage { get; set; }
    public decimal? ThresholdAmount { get; set; }
    public bool IsActive { get; set; } = true;
    public string NotificationEmail { get; set; } = string.Empty;
    public bool EmailEnabled { get; set; } = true;
    public bool PopupEnabled { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime LastTriggered { get; set; }
    public int TriggerCount { get; set; } = 0;
    
    // Navigation properties
    public virtual Material Material { get; set; } = null!;
    public virtual Province? Province { get; set; }
}