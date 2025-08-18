using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConstructionEstimator.Core.Enums;

namespace ConstructionEstimator.Core.Entities;

public class PriceList : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public PriceListType Type { get; set; }
    
    [MaxLength(20)]
    public string Currency { get; set; } = "VND";
    
    public DateTime EffectiveDate { get; set; }
    
    public DateTime? ExpiryDate { get; set; }
    
    [MaxLength(100)]
    public string? Region { get; set; }
    
    [MaxLength(100)]
    public string? Supplier { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public bool IsDefault { get; set; } = false;
    
    // Navigation properties
    public ICollection<PriceListItem> PriceListItems { get; set; } = new List<PriceListItem>();
}

public class PriceListItem : BaseEntity
{
    [Required]
    public int PriceListId { get; set; }
    
    public int? MaterialId { get; set; }
    
    public int? LaborId { get; set; }
    
    public int? EquipmentId { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }
    
    [Column(TypeName = "decimal(5,2)")]
    public decimal? DiscountPercentage { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? MinimumQuantity { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? MaximumQuantity { get; set; }
    
    public DateTime EffectiveDate { get; set; }
    
    public DateTime? ExpiryDate { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    // Navigation properties
    [ForeignKey("PriceListId")]
    public PriceList PriceList { get; set; } = null!;
    
    [ForeignKey("MaterialId")]
    public Material? Material { get; set; }
    
    [ForeignKey("LaborId")]
    public Labor? Labor { get; set; }
    
    [ForeignKey("EquipmentId")]
    public Equipment? Equipment { get; set; }
}