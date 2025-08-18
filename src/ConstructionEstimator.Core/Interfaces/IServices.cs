using ConstructionEstimator.Core.Entities;

namespace ConstructionEstimator.Core.Interfaces;

public interface IProjectService
{
    Task<Project> CreateProjectAsync(Project project);
    Task<Project?> GetProjectAsync(int id);
    Task<IEnumerable<Project>> GetAllProjectsAsync();
    Task<Project> UpdateProjectAsync(Project project);
    Task DeleteProjectAsync(int id);
    Task<Project?> GetProjectWithDetailsAsync(int id);
}

public interface IEstimationService
{
    decimal CalculateItemCost(EstimateItem item);
    decimal CalculateSectionCost(EstimateSection section);
    decimal CalculateProjectCost(Project project);
    Task<EstimateReport> GenerateReportAsync(Project project);
}

public interface IMaterialService
{
    Task<IEnumerable<Material>> GetAllMaterialsAsync();
    Task<IEnumerable<Material>> GetMaterialsByCategoryAsync(string category);
    Task<Material> CreateMaterialAsync(Material material);
    Task<Material> UpdateMaterialAsync(Material material);
    Task DeleteMaterialAsync(int id);
}

public interface ILaborService
{
    Task<IEnumerable<Labor>> GetAllLaborsAsync();
    Task<IEnumerable<Labor>> GetLaborsByCategoryAsync(string category);
    Task<Labor> CreateLaborAsync(Labor labor);
    Task<Labor> UpdateLaborAsync(Labor labor);
    Task DeleteLaborAsync(int id);
}

public interface IExportService
{
    Task<byte[]> ExportToExcelAsync(Project project);
    Task<byte[]> ExportToPdfAsync(Project project);
}

public class EstimateReport
{
    public Project Project { get; set; } = null!;
    public decimal TotalMaterialCost { get; set; }
    public decimal TotalLaborCost { get; set; }
    public decimal TotalEquipmentCost { get; set; }
    public decimal GrandTotal { get; set; }
    public DateTime GeneratedDate { get; set; }
    public List<SectionSummary> SectionSummaries { get; set; } = new();
}

public class SectionSummary
{
    public string SectionName { get; set; } = string.Empty;
    public string SectionCode { get; set; } = string.Empty;
    public decimal SectionTotal { get; set; }
    public int ItemCount { get; set; }
}