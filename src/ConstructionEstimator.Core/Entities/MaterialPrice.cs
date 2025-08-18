namespace ConstructionEstimator.Core.Entities;

public class MaterialPrice
{
    public int Id { get; set; }
    public int MaterialId { get; set; }
    public int ProvinceId { get; set; }
    public decimal UnitPrice { get; set; }
    public string Supplier { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string DataSourceUrl { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdated { get; set; }
    public string Notes { get; set; } = string.Empty;
    
    // Navigation properties
    public virtual Material Material { get; set; } = null!;
    public virtual Province Province { get; set; } = null!;
}