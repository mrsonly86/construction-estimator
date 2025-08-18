using ConstructionEstimator.Core.Entities;

namespace ConstructionEstimator.Core.Interfaces;

public interface IProjectRepository : IRepository<Project>
{
    Task<IEnumerable<Project>> GetProjectsByClientAsync(string clientName);
    Task<IEnumerable<Project>> GetProjectsByStatusAsync(Enums.ProjectStatus status);
    Task<Project?> GetProjectWithItemsAsync(int projectId);
}

public interface IMaterialRepository : IRepository<Material>
{
    Task<IEnumerable<Material>> GetMaterialsByCategoryAsync(Enums.MaterialCategory category);
    Task<IEnumerable<Material>> GetActiveMaterialsAsync();
    Task<Material?> GetMaterialByCodeAsync(string code);
}

public interface ILaborRepository : IRepository<Labor>
{
    Task<IEnumerable<Labor>> GetLaborByCategoryAsync(Enums.LaborCategory category);
    Task<IEnumerable<Labor>> GetActiveLaborAsync();
    Task<Labor?> GetLaborByCodeAsync(string code);
}

public interface IEquipmentRepository : IRepository<Equipment>
{
    Task<IEnumerable<Equipment>> GetActiveEquipmentAsync();
    Task<Equipment?> GetEquipmentByCodeAsync(string code);
}

public interface IStandardRepository : IRepository<Standard>
{
    Task<IEnumerable<Standard>> GetStandardsByCategoryAsync(string category);
    Task<IEnumerable<Standard>> GetActiveStandardsAsync();
    Task<Standard?> GetStandardByCodeAsync(string code);
    Task<Standard?> GetStandardWithItemsAsync(int standardId);
}

public interface IPriceListRepository : IRepository<PriceList>
{
    Task<IEnumerable<PriceList>> GetActivePriceListsAsync();
    Task<PriceList?> GetDefaultPriceListAsync(Enums.PriceListType type);
    Task<PriceList?> GetPriceListWithItemsAsync(int priceListId);
}

public interface IEstimateItemRepository : IRepository<EstimateItem>
{
    Task<IEnumerable<EstimateItem>> GetItemsByProjectAsync(int projectId);
    Task<IEnumerable<EstimateItem>> GetItemsByTypeAsync(Enums.EstimateItemType type);
}

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetCategoriesByTypeAsync(string type);
    Task<IEnumerable<Category>> GetRootCategoriesAsync(string type);
    Task<Category?> GetCategoryWithSubCategoriesAsync(int categoryId);
}