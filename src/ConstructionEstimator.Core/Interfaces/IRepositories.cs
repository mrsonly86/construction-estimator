using ConstructionEstimator.Core.Entities;

namespace ConstructionEstimator.Core.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}

public interface IProjectRepository : IRepository<Project>
{
    Task<IEnumerable<Project>> GetProjectsByStatusAsync(Enums.ProjectStatus status);
    Task<Project?> GetProjectWithSectionsAsync(int projectId);
}

public interface IMaterialRepository : IRepository<Material>
{
    Task<IEnumerable<Material>> GetMaterialsByCategoryAsync(string category);
    Task<IEnumerable<Material>> SearchMaterialsAsync(string searchTerm);
}

public interface ILaborRepository : IRepository<Labor>
{
    Task<IEnumerable<Labor>> GetLaborsByCategoryAsync(string category);
    Task<IEnumerable<Labor>> GetLaborsBySkillLevelAsync(Enums.LaborSkillLevel skillLevel);
}

// New repository interfaces for Material Price Auto-Update System
public interface IProvinceRepository : IRepository<Province>
{
    Task<Province?> GetProvinceByCodeAsync(string code);
    Task<IEnumerable<Province>> GetProvincesByRegionAsync(string region);
    Task<IEnumerable<Province>> GetActiveProvincesAsync();
}

public interface IMaterialPriceRepository : IRepository<MaterialPrice>
{
    Task<IEnumerable<MaterialPrice>> GetMaterialPricesByProvinceAsync(int provinceId);
    Task<IEnumerable<MaterialPrice>> GetMaterialPricesForMaterialAsync(int materialId, int? provinceId = null);
    Task<MaterialPrice?> GetCurrentPriceAsync(int materialId, int provinceId);
    Task<IEnumerable<MaterialPrice>> GetPricesEffectiveOnDateAsync(DateTime date, int? materialId = null, int? provinceId = null);
}

public interface IPriceHistoryRepository : IRepository<PriceHistory>
{
    Task<IEnumerable<PriceHistory>> GetPriceHistoryAsync(int materialId, int? provinceId = null);
    Task<IEnumerable<PriceHistory>> GetRecentPriceChangesAsync(DateTime fromDate, int? provinceId = null);
    Task<IEnumerable<PriceHistory>> GetSignificantPriceChangesAsync(decimal thresholdPercentage, DateTime fromDate);
    Task<decimal> GetAveragePriceChangeAsync(int materialId, int days);
}

public interface IPriceAlertRepository : IRepository<PriceAlert>
{
    Task<IEnumerable<PriceAlert>> GetActiveAlertsAsync();
    Task<IEnumerable<PriceAlert>> GetAlertsForMaterialAsync(int materialId);
    Task<IEnumerable<PriceAlert>> GetAlertsForProvinceAsync(int provinceId);
}

public interface IDataSourceRepository : IRepository<DataSource>
{
    Task<IEnumerable<DataSource>> GetDataSourcesByProvinceAsync(int provinceId);
    Task<IEnumerable<DataSource>> GetActiveDataSourcesAsync();
    Task<IEnumerable<DataSource>> GetDataSourcesDueForUpdateAsync();
    Task<IEnumerable<DataSource>> GetDataSourcesByTypeAsync(string sourceType);
}