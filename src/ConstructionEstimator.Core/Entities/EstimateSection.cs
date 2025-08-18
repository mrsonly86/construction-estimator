namespace ConstructionEstimator.Core.Entities;

public class EstimateSection
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    
    public Project Project { get; set; } = null!;
    public List<EstimateItem> Items { get; set; } = new();
    
    public decimal TotalCost => Items?.Sum(i => i.TotalCost) ?? 0;
}