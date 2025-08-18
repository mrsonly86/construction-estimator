namespace ConstructionEstimator.Core.Entities;

public class Province
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // e.g., "HN" for Hà Nội
    public string Region { get; set; } = string.Empty; // Miền Bắc, Miền Trung, Miền Nam
    public bool IsActive { get; set; } = true;
    public DateTime LastUpdated { get; set; }
    
    // Navigation properties
    public virtual ICollection<MaterialPrice> MaterialPrices { get; set; } = new List<MaterialPrice>();
    public virtual ICollection<DataSource> DataSources { get; set; } = new List<DataSource>();
}