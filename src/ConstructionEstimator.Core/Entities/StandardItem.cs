using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructionEstimator.Core.Entities;

public class StandardItem : BaseEntity
{
    [Required]
    public int StandardId { get; set; }
    
    public int? MaterialId { get; set; }
    
    public int? LaborId { get; set; }
    
    public int? EquipmentId { get; set; }
    
    [Column(TypeName = "decimal(18,4)")]
    public decimal Quantity { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? UnitPrice { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    // Navigation properties
    [ForeignKey("StandardId")]
    public Standard Standard { get; set; } = null!;
    
    [ForeignKey("MaterialId")]
    public Material? Material { get; set; }
    
    [ForeignKey("LaborId")]
    public Labor? Labor { get; set; }
    
    [ForeignKey("EquipmentId")]
    public Equipment? Equipment { get; set; }
}