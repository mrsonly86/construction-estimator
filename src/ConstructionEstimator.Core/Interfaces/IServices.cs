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

// New interfaces for Material Price Auto-Update System
public interface IMaterialPriceUpdateService
{
    Task<bool> UpdatePricesForProvinceAsync(int provinceId);
    Task<bool> UpdatePricesForMaterialAsync(int materialId);
    Task<bool> UpdateAllPricesAsync();
    Task<IEnumerable<MaterialPrice>> GetMaterialPricesByProvinceAsync(int provinceId);
    Task<IEnumerable<MaterialPrice>> GetMaterialPricesForMaterialAsync(int materialId, int? provinceId = null);
    Task<MaterialPrice?> GetCurrentPriceAsync(int materialId, int provinceId);
}

public interface IProvinceConfigService
{
    Task<IEnumerable<Province>> GetAllProvincesAsync();
    Task<Province?> GetProvinceAsync(int id);
    Task<Province?> GetProvinceByCodeAsync(string code);
    Task<Province> CreateProvinceAsync(Province province);
    Task<Province> UpdateProvinceAsync(Province province);
    Task DeleteProvinceAsync(int id);
    Task<IEnumerable<Province>> GetProvincesByRegionAsync(string region);
}

public interface IPriceHistoryService
{
    Task<IEnumerable<PriceHistory>> GetPriceHistoryAsync(int materialId, int? provinceId = null);
    Task<PriceHistory> RecordPriceChangeAsync(int materialPriceId, decimal oldPrice, decimal newPrice, string source);
    Task<IEnumerable<PriceHistory>> GetRecentPriceChangesAsync(DateTime fromDate, int? provinceId = null);
    Task<decimal> GetAveragePriceChangeAsync(int materialId, int days);
    Task<IEnumerable<PriceHistory>> GetSignificantPriceChangesAsync(decimal thresholdPercentage, DateTime fromDate);
}

public interface INotificationService
{
    Task SendPriceAlertAsync(PriceAlert alert, PriceHistory priceChange);
    Task<IEnumerable<PriceAlert>> GetActiveAlertsAsync();
    Task<PriceAlert> CreatePriceAlertAsync(PriceAlert alert);
    Task<PriceAlert> UpdatePriceAlertAsync(PriceAlert alert);
    Task DeletePriceAlertAsync(int id);
    Task CheckAndTriggerAlertsAsync(int materialId, int provinceId, decimal priceChange);
}

public interface IDataSourceService
{
    Task<IEnumerable<DataSource>> GetDataSourcesByProvinceAsync(int provinceId);
    Task<DataSource?> GetDataSourceAsync(int id);
    Task<DataSource> CreateDataSourceAsync(DataSource dataSource);
    Task<DataSource> UpdateDataSourceAsync(DataSource dataSource);
    Task DeleteDataSourceAsync(int id);
    Task<IEnumerable<DataSource>> GetActiveDataSourcesAsync();
    Task<IEnumerable<DataSource>> GetDataSourcesDueForUpdateAsync();
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