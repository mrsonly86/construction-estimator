namespace ConstructionEstimator.Core.Entities;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Location { get; set; } = string.Empty;
    public string ProvinceCode { get; set; } = string.Empty; // Mã tỉnh/thành (01-63)
    public DateTime CreatedDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal EstimatedBudget { get; set; }
    public decimal ActualCost { get; set; }
    public ProjectStatus Status { get; set; }
    
    // Navigation properties
    public virtual ICollection<ProjectMaterial> ProjectMaterials { get; set; } = new List<ProjectMaterial>();
    public virtual ICollection<CostEstimate> CostEstimates { get; set; } = new List<CostEstimate>();
}

public enum ProjectStatus
{
    Planning = 0,     // Đang lập kế hoạch
    InProgress = 1,   // Đang thực hiện
    Completed = 2,    // Hoàn thành
    Cancelled = 3,    // Đã hủy
    OnHold = 4        // Tạm dừng
}