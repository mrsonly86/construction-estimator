using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructionEstimator.Core.Entities;

public class Standard : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? SubCategory { get; set; }
    
    [Column(TypeName = "decimal(18,4)")]
    public decimal ProductivityRate { get; set; }
    
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;
    
    [Column(TypeName = "decimal(8,4)")]
    public decimal? MaterialQuantity { get; set; }
    
    [Column(TypeName = "decimal(8,4)")]
    public decimal? LaborHours { get; set; }
    
    [Column(TypeName = "decimal(8,4)")]
    public decimal? EquipmentHours { get; set; }
    
    [MaxLength(1000)]
    public string? Formula { get; set; }
    
    [MaxLength(500)]
    public string? Conditions { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    [MaxLength(100)]
    public string? Source { get; set; }
    
    public DateTime? EffectiveDate { get; set; }
    
    public DateTime? ExpiryDate { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<EstimateItem> EstimateItems { get; set; } = new List<EstimateItem>();
    public ICollection<StandardItem> StandardItems { get; set; } = new List<StandardItem>();
}