namespace ConstructionEstimator.Core.Entities;

public class DataSource
{
    public int Id { get; set; }
    public int ProvinceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SourceType { get; set; } = string.Empty; // "URL", "Excel", "PDF", "API"
    public string SourceUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int UpdateFrequencyDays { get; set; } = 30; // Default monthly
    public DateTime LastScanDate { get; set; }
    public DateTime NextScanDate { get; set; }
    public string ScanConfiguration { get; set; } = string.Empty; // JSON configuration for scraping
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdated { get; set; }
    
    // Navigation properties
    public virtual Province Province { get; set; } = null!;
}