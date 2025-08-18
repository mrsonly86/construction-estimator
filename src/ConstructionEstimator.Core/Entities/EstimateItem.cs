namespace ConstructionEstimator.Core.Entities;

public class EstimateItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal MaterialCost { get; set; }
    public decimal LaborCost { get; set; }
    public decimal EquipmentCost { get; set; }
    public int SectionId { get; set; }
    
    public EstimateSection Section { get; set; } = null!;
    
    public decimal UnitPrice => MaterialCost + LaborCost + EquipmentCost;
    public decimal TotalCost => Quantity * UnitPrice;
}