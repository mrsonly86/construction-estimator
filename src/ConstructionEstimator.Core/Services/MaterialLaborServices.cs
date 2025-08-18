using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Interfaces;

namespace ConstructionEstimator.Core.Services;

public class MaterialService : IMaterialService
{
    private readonly IMaterialRepository _materialRepository;

    public MaterialService(IMaterialRepository materialRepository)
    {
        _materialRepository = materialRepository;
    }

    public async Task<IEnumerable<Material>> GetAllMaterialsAsync()
    {
        return await _materialRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Material>> GetMaterialsByCategoryAsync(string category)
    {
        return await _materialRepository.GetMaterialsByCategoryAsync(category);
    }

    public async Task<Material> CreateMaterialAsync(Material material)
    {
        material.LastUpdated = DateTime.Now;
        await _materialRepository.AddAsync(material);
        await _materialRepository.SaveChangesAsync();
        return material;
    }

    public async Task<Material> UpdateMaterialAsync(Material material)
    {
        material.LastUpdated = DateTime.Now;
        await _materialRepository.UpdateAsync(material);
        await _materialRepository.SaveChangesAsync();
        return material;
    }

    public async Task DeleteMaterialAsync(int id)
    {
        await _materialRepository.DeleteAsync(id);
        await _materialRepository.SaveChangesAsync();
    }
}

public class LaborService : ILaborService
{
    private readonly ILaborRepository _laborRepository;

    public LaborService(ILaborRepository laborRepository)
    {
        _laborRepository = laborRepository;
    }

    public async Task<IEnumerable<Labor>> GetAllLaborsAsync()
    {
        return await _laborRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Labor>> GetLaborsByCategoryAsync(string category)
    {
        return await _laborRepository.GetLaborsByCategoryAsync(category);
    }

    public async Task<Labor> CreateLaborAsync(Labor labor)
    {
        await _laborRepository.AddAsync(labor);
        await _laborRepository.SaveChangesAsync();
        return labor;
    }

    public async Task<Labor> UpdateLaborAsync(Labor labor)
    {
        await _laborRepository.UpdateAsync(labor);
        await _laborRepository.SaveChangesAsync();
        return labor;
    }

    public async Task DeleteLaborAsync(int id)
    {
        await _laborRepository.DeleteAsync(id);
        await _laborRepository.SaveChangesAsync();
    }
}