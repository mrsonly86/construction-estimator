namespace ConstructionEstimator.Core.Entities;

public class PriceHistory
{
    public int Id { get; set; }
    public int MaterialPriceId { get; set; }
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public decimal ChangeAmount { get; set; }
    public decimal ChangePercentage { get; set; }
    public string ChangeType { get; set; } = string.Empty; // "Increase", "Decrease", "Stable"
    public DateTime ChangeDate { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    
    // Navigation properties
    public virtual MaterialPrice MaterialPrice { get; set; } = null!;
}