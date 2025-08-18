namespace ConstructionEstimator.Core.Models;

public class MaterialPrice
{
    public int Id { get; set; }
    public string MaterialName { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
    public DateTime LastUpdated { get; set; }
    public string? Source { get; set; }
    public string? Notes { get; set; }
}