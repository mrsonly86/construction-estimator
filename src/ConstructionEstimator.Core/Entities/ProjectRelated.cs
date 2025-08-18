namespace ConstructionEstimator.Core.Entities;

public class ProjectMaterial
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int MaterialId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime DateAdded { get; set; }
    public string? Notes { get; set; }
    
    // Navigation properties
    public virtual Project Project { get; set; } = null!;
    public virtual Material Material { get; set; } = null!;
}

public class CostEstimate
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal MaterialCost { get; set; }
    public decimal LaborCost { get; set; }
    public decimal EquipmentCost { get; set; }
    public decimal OverheadCost { get; set; }
    public decimal ProfitMargin { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsBaseline { get; set; } = false;
    public string? Notes { get; set; }
    
    // Navigation properties
    public virtual Project Project { get; set; } = null!;
}

public class PriceHistory
{
    public int Id { get; set; }
    public int MaterialId { get; set; }
    public string ProvinceCode { get; set; } = string.Empty;
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public decimal ChangePercentage { get; set; }
    public DateTime ChangeDate { get; set; }
    public string Source { get; set; } = string.Empty;
    public PriceChangeType ChangeType { get; set; }
    
    // Navigation properties
    public virtual Material Material { get; set; } = null!;
    public virtual Province Province { get; set; } = null!;
}

public enum PriceChangeType
{
    Increase = 1,    // Tăng giá
    Decrease = 2,    // Giảm giá
    NoChange = 3     // Không đổi
}