namespace ConstructionEstimator.Core.Entities;

public class MaterialPrice
{
    public int Id { get; set; }
    public int MaterialId { get; set; }
    public string ProvinceCode { get; set; } = string.Empty; // Mã tỉnh/thành (01-63)
    public decimal Price { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Source { get; set; } = string.Empty; // Nguồn: "SXD_HaNoi", "SXD_HoChiMinh", etc.
    public string? SourceUrl { get; set; }
    public DateTime LastUpdated { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual Material Material { get; set; } = null!;
    public virtual Province Province { get; set; } = null!;
}