using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConstructionEstimator.Core.Enums;

namespace ConstructionEstimator.Core.Entities;

public class Equipment : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? Type { get; set; }
    
    [MaxLength(100)]
    public string? Manufacturer { get; set; }
    
    [MaxLength(100)]
    public string? Model { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal HourlyRate { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal DailyRate { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal MonthlyRate { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal FuelConsumption { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal MaintenanceCostPerHour { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal OperatorCostPerHour { get; set; }
    
    [Column(TypeName = "decimal(8,2)")]
    public decimal Capacity { get; set; }
    
    [MaxLength(50)]
    public string? CapacityUnit { get; set; }
    
    [Column(TypeName = "decimal(8,2)")]
    public decimal? Power { get; set; }
    
    [MaxLength(20)]
    public string? PowerUnit { get; set; }
    
    [Column(TypeName = "decimal(8,2)")]
    public decimal? Weight { get; set; }
    
    [MaxLength(500)]
    public string? Specifications { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime? LastRateUpdate { get; set; }
    
    // Navigation properties
    public ICollection<EstimateItem> EstimateItems { get; set; } = new List<EstimateItem>();
    public ICollection<PriceListItem> PriceListItems { get; set; } = new List<PriceListItem>();
}