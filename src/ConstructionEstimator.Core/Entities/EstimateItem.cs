using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConstructionEstimator.Core.Enums;

namespace ConstructionEstimator.Core.Entities;

public class EstimateItem : BaseEntity
{
    [Required]
    public int ProjectId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;
    
    public EstimateItemType Type { get; set; }
    
    public UnitOfMeasure UnitOfMeasure { get; set; }
    
    [Column(TypeName = "decimal(18,4)")]
    public decimal Quantity { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }
    
    public int? MaterialId { get; set; }
    
    public int? LaborId { get; set; }
    
    public int? EquipmentId { get; set; }
    
    public int? StandardId { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    public int? ParentItemId { get; set; }
    
    public int SortOrder { get; set; }
    
    // Navigation properties
    [ForeignKey("ProjectId")]
    public Project Project { get; set; } = null!;
    
    [ForeignKey("MaterialId")]
    public Material? Material { get; set; }
    
    [ForeignKey("LaborId")]
    public Labor? Labor { get; set; }
    
    [ForeignKey("EquipmentId")]
    public Equipment? Equipment { get; set; }
    
    [ForeignKey("StandardId")]
    public Standard? Standard { get; set; }
    
    [ForeignKey("ParentItemId")]
    public EstimateItem? ParentItem { get; set; }
    
    public ICollection<EstimateItem> SubItems { get; set; } = new List<EstimateItem>();
}