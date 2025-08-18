namespace ConstructionEstimator.Core.Entities;

public class Material
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; } // Default national average price
    public string Supplier { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Auto-update configuration
    public bool EnableAutoUpdate { get; set; } = true;
    public string Keywords { get; set; } = string.Empty; // Keywords for price matching during scraping
    
    // Navigation properties for province-specific pricing
    public virtual ICollection<MaterialPrice> ProvincesPrices { get; set; } = new List<MaterialPrice>();
    public virtual ICollection<PriceAlert> PriceAlerts { get; set; } = new List<PriceAlert>();
}