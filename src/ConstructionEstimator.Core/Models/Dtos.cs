using ConstructionEstimator.Core.Enums;

namespace ConstructionEstimator.Core.Models;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string? ClientAddress { get; set; }
    public string? ClientPhone { get; set; }
    public string? ClientEmail { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectStatus Status { get; set; }
    public string Currency { get; set; } = "VND";
    public decimal TotalCost { get; set; }
    public decimal LaborCost { get; set; }
    public decimal MaterialCost { get; set; }
    public decimal EquipmentCost { get; set; }
    public decimal OverheadCost { get; set; }
    public decimal ProfitAmount { get; set; }
    public decimal ProfitPercentage { get; set; }
    public decimal ContingencyPercentage { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<EstimateItemDto> EstimateItems { get; set; } = new();
}

public class EstimateItemDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Code { get; set; } = string.Empty;
    public EstimateItemType Type { get; set; }
    public UnitOfMeasure UnitOfMeasure { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public int? MaterialId { get; set; }
    public int? LaborId { get; set; }
    public int? EquipmentId { get; set; }
    public int? StandardId { get; set; }
    public string? Notes { get; set; }
    public int? ParentItemId { get; set; }
    public int SortOrder { get; set; }
    public MaterialDto? Material { get; set; }
    public LaborDto? Labor { get; set; }
    public EquipmentDto? Equipment { get; set; }
    public List<EstimateItemDto> SubItems { get; set; } = new();
}

public class MaterialDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Code { get; set; } = string.Empty;
    public MaterialCategory Category { get; set; }
    public UnitOfMeasure UnitOfMeasure { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Supplier { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Specifications { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Volume { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastPriceUpdate { get; set; }
}

public class LaborDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Code { get; set; } = string.Empty;
    public LaborCategory Category { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal DailyRate { get; set; }
    public decimal MonthlyRate { get; set; }
    public decimal ProductivityFactor { get; set; }
    public string? SkillRequirements { get; set; }
    public string? SafetyRequirements { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastRateUpdate { get; set; }
}

public class EquipmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Type { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal DailyRate { get; set; }
    public decimal MonthlyRate { get; set; }
    public decimal FuelConsumption { get; set; }
    public decimal MaintenanceCostPerHour { get; set; }
    public decimal OperatorCostPerHour { get; set; }
    public decimal Capacity { get; set; }
    public string? CapacityUnit { get; set; }
    public decimal? Power { get; set; }
    public string? PowerUnit { get; set; }
    public decimal? Weight { get; set; }
    public string? Specifications { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastRateUpdate { get; set; }
}