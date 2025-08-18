using ConstructionEstimator.Core.Models;

namespace ConstructionEstimator.Reports.Interfaces;

public interface IReportGenerator
{
    Task<byte[]> GenerateEstimateReportAsync(ProjectDto project);
    Task<byte[]> GenerateMaterialListAsync(ProjectDto project);
    Task<byte[]> GenerateCostSummaryAsync(ProjectDto project);
    Task<string> GenerateEstimateReportHtmlAsync(ProjectDto project);
}

public interface IExcelExporter
{
    Task<byte[]> ExportProjectToExcelAsync(ProjectDto project);
    Task<byte[]> ExportMaterialListToExcelAsync(IEnumerable<MaterialDto> materials);
}

public interface IPdfExporter
{
    Task<byte[]> ExportEstimateToPdfAsync(ProjectDto project);
}