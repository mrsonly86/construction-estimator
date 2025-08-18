using ConstructionEstimator.Core.Enums;

namespace ConstructionEstimator.Core.Entities;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Client { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public ProjectStatus Status { get; set; }
    public decimal TotalEstimatedCost { get; set; }
    
    public List<EstimateSection> Sections { get; set; } = new();
}