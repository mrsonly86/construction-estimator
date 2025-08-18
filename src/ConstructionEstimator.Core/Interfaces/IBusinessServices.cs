using ConstructionEstimator.Core.Entities;

namespace ConstructionEstimator.Core.Interfaces;

public interface IProjectService
{
    Task<Project?> GetByIdAsync(int id);
    Task<IEnumerable<Project>> GetAllAsync();
    Task<IEnumerable<Project>> GetByProvinceAsync(string provinceCode);
    Task<Project> CreateAsync(Project project);
    Task<bool> UpdateAsync(Project project);
    Task<bool> DeleteAsync(int id);
    Task<decimal> CalculateEstimatedCostAsync(int projectId);
}

public interface IMaterialService
{
    Task<Material?> GetByIdAsync(int id);
    Task<IEnumerable<Material>> GetAllAsync();
    Task<IEnumerable<Material>> GetByCategoryAsync(MaterialCategory category);
    Task<IEnumerable<Material>> SearchAsync(string keyword);
    Task<Material> CreateAsync(Material material);
    Task<bool> UpdateAsync(Material material);
    Task<bool> DeleteAsync(int id);
}

public interface IMaterialPriceService
{
    Task<MaterialPrice?> GetCurrentPriceAsync(int materialId, string provinceCode);
    Task<IEnumerable<MaterialPrice>> GetPricesForMaterialAsync(int materialId);
    Task<IEnumerable<MaterialPrice>> GetPricesForProvinceAsync(string provinceCode);
    Task<MaterialPrice> CreateAsync(MaterialPrice materialPrice);
    Task<bool> UpdateAsync(MaterialPrice materialPrice);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<MaterialPrice>> GetPriceComparisonAsync(int materialId, IEnumerable<string> provinceCodes);
}

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

public interface IUnitOfWork : IDisposable
{
    IRepository<Project> Projects { get; }
    IRepository<Material> Materials { get; }
    IRepository<MaterialPrice> MaterialPrices { get; }
    IRepository<Province> Provinces { get; }
    IRepository<ProvinceConfig> ProvinceConfigs { get; }
    IRepository<PriceHistory> PriceHistories { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}