using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConstructionEstimator.Core.Enums;

namespace ConstructionEstimator.Core.Entities;

public class Material : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;
    
    public MaterialCategory Category { get; set; }
    
    public UnitOfMeasure UnitOfMeasure { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }
    
    [MaxLength(100)]
    public string? Supplier { get; set; }
    
    [MaxLength(50)]
    public string? Brand { get; set; }
    
    [MaxLength(100)]
    public string? Model { get; set; }
    
    [MaxLength(500)]
    public string? Specifications { get; set; }
    
    [Column(TypeName = "decimal(8,4)")]
    public decimal? Weight { get; set; }
    
    [Column(TypeName = "decimal(8,4)")]
    public decimal? Volume { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime? LastPriceUpdate { get; set; }
    
    // Navigation properties
    public ICollection<EstimateItem> EstimateItems { get; set; } = new List<EstimateItem>();
    public ICollection<PriceListItem> PriceListItems { get; set; } = new List<PriceListItem>();
}