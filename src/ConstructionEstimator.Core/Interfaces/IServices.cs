using ConstructionEstimator.Core.Models;

namespace ConstructionEstimator.Core.Services
{
    public interface IProjectService
    {
        Task<Project?> GetProjectByIdAsync(int id);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project> CreateProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(int id);
        Task<decimal> CalculateProjectTotalCostAsync(int projectId);
    }

    public interface IMaterialService
    {
        Task<Material?> GetMaterialByIdAsync(int id);
        Task<Material?> GetMaterialByCodeAsync(string code);
        Task<IEnumerable<Material>> GetAllMaterialsAsync();
        Task<IEnumerable<Material>> GetMaterialsByCategoryAsync(MaterialCategory category);
        Task<Material> CreateMaterialAsync(Material material);
        Task<Material> UpdateMaterialAsync(Material material);
        Task<bool> DeleteMaterialAsync(int id);
        Task<decimal> GetCurrentPriceAsync(int materialId, string? region = null);
        Task UpdateMaterialPriceAsync(int materialId, decimal price, string? region = null, string? supplier = null);
    }

    public interface ILaborService
    {
        Task<Labor?> GetLaborByIdAsync(int id);
        Task<Labor?> GetLaborByCodeAsync(string code);
        Task<IEnumerable<Labor>> GetAllLaborAsync();
        Task<IEnumerable<Labor>> GetLaborByTypeAsync(LaborType type);
        Task<Labor> CreateLaborAsync(Labor labor);
        Task<Labor> UpdateLaborAsync(Labor labor);
        Task<bool> DeleteLaborAsync(int id);
        Task<decimal> GetCurrentCostAsync(int laborId, string? region = null);
        Task UpdateLaborCostAsync(int laborId, decimal cost, string? region = null);
    }

    public interface IEquipmentService
    {
        Task<Equipment?> GetEquipmentByIdAsync(int id);
        Task<Equipment?> GetEquipmentByCodeAsync(string code);
        Task<IEnumerable<Equipment>> GetAllEquipmentAsync();
        Task<IEnumerable<Equipment>> GetEquipmentByCategoryAsync(EquipmentCategory category);
        Task<Equipment> CreateEquipmentAsync(Equipment equipment);
        Task<Equipment> UpdateEquipmentAsync(Equipment equipment);
        Task<bool> DeleteEquipmentAsync(int id);
        Task<decimal> GetCurrentCostAsync(int equipmentId, string? region = null);
        Task UpdateEquipmentCostAsync(int equipmentId, decimal cost, string? region = null, string? supplier = null);
    }

    public interface IEstimateService
    {
        Task<EstimateItem?> GetEstimateItemByIdAsync(int id);
        Task<IEnumerable<EstimateItem>> GetEstimateItemsByProjectIdAsync(int projectId);
        Task<EstimateItem> CreateEstimateItemAsync(EstimateItem estimateItem);
        Task<EstimateItem> UpdateEstimateItemAsync(EstimateItem estimateItem);
        Task<bool> DeleteEstimateItemAsync(int id);
        Task<decimal> CalculateEstimateItemTotalCostAsync(int estimateItemId);
        Task ApplyStandardToEstimateItemAsync(int estimateItemId, int standardId);
    }

    public interface IStandardService
    {
        Task<Standard?> GetStandardByIdAsync(int id);
        Task<Standard?> GetStandardByCodeAsync(string code);
        Task<IEnumerable<Standard>> GetAllStandardsAsync();
        Task<IEnumerable<Standard>> GetStandardsByCategoryAsync(StandardCategory category);
        Task<Standard> CreateStandardAsync(Standard standard);
        Task<Standard> UpdateStandardAsync(Standard standard);
        Task<bool> DeleteStandardAsync(int id);
    }
}