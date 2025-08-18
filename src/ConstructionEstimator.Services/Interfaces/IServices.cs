using ConstructionEstimator.Core.Models;
using ConstructionEstimator.Core.Enums;

namespace ConstructionEstimator.Services.Interfaces;

public interface IProjectService
{
    Task<ProjectDto> CreateProjectAsync(ProjectDto projectDto);
    Task<ProjectDto?> GetProjectByIdAsync(int id);
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task<IEnumerable<ProjectDto>> GetProjectsByClientAsync(string clientName);
    Task<IEnumerable<ProjectDto>> GetProjectsByStatusAsync(ProjectStatus status);
    Task<ProjectDto> UpdateProjectAsync(ProjectDto projectDto);
    Task DeleteProjectAsync(int id);
    Task<ProjectDto?> GetProjectWithItemsAsync(int projectId);
    Task<decimal> CalculateProjectTotalAsync(int projectId);
}

public interface IEstimationService
{
    Task<EstimateItemDto> CreateEstimateItemAsync(EstimateItemDto itemDto);
    Task<EstimateItemDto> UpdateEstimateItemAsync(EstimateItemDto itemDto);
    Task DeleteEstimateItemAsync(int id);
    Task<IEnumerable<EstimateItemDto>> GetEstimateItemsByProjectAsync(int projectId);
    Task<decimal> CalculateItemTotalAsync(EstimateItemDto item);
    Task<decimal> CalculateProjectCostBreakdownAsync(int projectId);
    Task<Dictionary<string, decimal>> GetCostSummaryAsync(int projectId);
}

public interface IMaterialService
{
    Task<MaterialDto> CreateMaterialAsync(MaterialDto materialDto);
    Task<MaterialDto?> GetMaterialByIdAsync(int id);
    Task<IEnumerable<MaterialDto>> GetAllMaterialsAsync();
    Task<IEnumerable<MaterialDto>> GetMaterialsByCategoryAsync(MaterialCategory category);
    Task<IEnumerable<MaterialDto>> GetActiveMaterialsAsync();
    Task<MaterialDto> UpdateMaterialAsync(MaterialDto materialDto);
    Task DeleteMaterialAsync(int id);
    Task<MaterialDto?> GetMaterialByCodeAsync(string code);
}

public interface ILaborService
{
    Task<LaborDto> CreateLaborAsync(LaborDto laborDto);
    Task<LaborDto?> GetLaborByIdAsync(int id);
    Task<IEnumerable<LaborDto>> GetAllLaborAsync();
    Task<IEnumerable<LaborDto>> GetLaborByCategoryAsync(LaborCategory category);
    Task<IEnumerable<LaborDto>> GetActiveLaborAsync();
    Task<LaborDto> UpdateLaborAsync(LaborDto laborDto);
    Task DeleteLaborAsync(int id);
    Task<LaborDto?> GetLaborByCodeAsync(string code);
}

public interface IEquipmentService
{
    Task<EquipmentDto> CreateEquipmentAsync(EquipmentDto equipmentDto);
    Task<EquipmentDto?> GetEquipmentByIdAsync(int id);
    Task<IEnumerable<EquipmentDto>> GetAllEquipmentAsync();
    Task<IEnumerable<EquipmentDto>> GetActiveEquipmentAsync();
    Task<EquipmentDto> UpdateEquipmentAsync(EquipmentDto equipmentDto);
    Task DeleteEquipmentAsync(int id);
    Task<EquipmentDto?> GetEquipmentByCodeAsync(string code);
}

public interface ICalculationService
{
    decimal CalculateItemTotal(decimal quantity, decimal unitPrice);
    decimal CalculateWithContingency(decimal amount, decimal contingencyPercentage);
    decimal CalculateWithProfit(decimal amount, decimal profitPercentage);
    decimal CalculateOverhead(decimal laborCost, decimal overheadPercentage);
    Dictionary<string, decimal> CalculateCostBreakdown(decimal materialCost, decimal laborCost, decimal equipmentCost, decimal profitPercentage, decimal contingencyPercentage);
}

public interface IPriceService
{
    Task<decimal> GetLatestMaterialPriceAsync(int materialId);
    Task<decimal> GetLatestLaborRateAsync(int laborId);
    Task<decimal> GetLatestEquipmentRateAsync(int equipmentId);
    Task UpdateMaterialPriceAsync(int materialId, decimal newPrice);
    Task UpdateLaborRateAsync(int laborId, decimal newRate);
    Task UpdateEquipmentRateAsync(int equipmentId, decimal newRate);
}