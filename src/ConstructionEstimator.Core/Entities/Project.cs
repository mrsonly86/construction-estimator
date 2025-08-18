using System.ComponentModel.DataAnnotations;
using ConstructionEstimator.Core.Enums;

namespace ConstructionEstimator.Core.Entities;

public class Project : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string ClientName { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? ClientAddress { get; set; }
    
    [MaxLength(50)]
    public string? ClientPhone { get; set; }
    
    [MaxLength(100)]
    public string? ClientEmail { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string Location { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public ProjectStatus Status { get; set; } = ProjectStatus.Draft;
    
    [MaxLength(20)]
    public string Currency { get; set; } = "VND";
    
    public decimal TotalCost { get; set; }
    
    public decimal LaborCost { get; set; }
    
    public decimal MaterialCost { get; set; }
    
    public decimal EquipmentCost { get; set; }
    
    public decimal OverheadCost { get; set; }
    
    public decimal ProfitAmount { get; set; }
    
    public decimal ProfitPercentage { get; set; } = 10;
    
    public decimal ContingencyPercentage { get; set; } = 5;
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    // Navigation properties
    public ICollection<EstimateItem> EstimateItems { get; set; } = new List<EstimateItem>();
}