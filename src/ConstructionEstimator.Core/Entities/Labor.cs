using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConstructionEstimator.Core.Enums;

namespace ConstructionEstimator.Core.Entities;

public class Labor : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;
    
    public LaborCategory Category { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal HourlyRate { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal DailyRate { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal MonthlyRate { get; set; }
    
    [Column(TypeName = "decimal(5,2)")]
    public decimal ProductivityFactor { get; set; } = 1.0m;
    
    [MaxLength(500)]
    public string? SkillRequirements { get; set; }
    
    [MaxLength(500)]
    public string? SafetyRequirements { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime? LastRateUpdate { get; set; }
    
    // Navigation properties
    public ICollection<EstimateItem> EstimateItems { get; set; } = new List<EstimateItem>();
    public ICollection<PriceListItem> PriceListItems { get; set; } = new List<PriceListItem>();
}