using AutoMapper;
using Microsoft.Extensions.Logging;
using ConstructionEstimator.Core.Interfaces;
using ConstructionEstimator.Core.Models;
using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Enums;
using ConstructionEstimator.Services.Interfaces;

namespace ConstructionEstimator.Services.Implementations;

public class MaterialService : IMaterialService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<MaterialService> _logger;

    public MaterialService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MaterialService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MaterialDto> CreateMaterialAsync(MaterialDto materialDto)
    {
        try
        {
            var material = _mapper.Map<Material>(materialDto);
            material.CreatedAt = DateTime.UtcNow;
            material.LastPriceUpdate = DateTime.UtcNow;

            await _unitOfWork.Materials.AddAsync(material);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Created new material: {MaterialName} with ID: {MaterialId}", material.Name, material.Id);
            
            return _mapper.Map<MaterialDto>(material);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating material: {MaterialName}", materialDto.Name);
            throw;
        }
    }

    public async Task<MaterialDto?> GetMaterialByIdAsync(int id)
    {
        var material = await _unitOfWork.Materials.GetByIdAsync(id);
        return material != null ? _mapper.Map<MaterialDto>(material) : null;
    }

    public async Task<IEnumerable<MaterialDto>> GetAllMaterialsAsync()
    {
        var materials = await _unitOfWork.Materials.GetAllAsync();
        return _mapper.Map<IEnumerable<MaterialDto>>(materials);
    }

    public async Task<IEnumerable<MaterialDto>> GetMaterialsByCategoryAsync(MaterialCategory category)
    {
        var materials = await _unitOfWork.Materials.GetMaterialsByCategoryAsync(category);
        return _mapper.Map<IEnumerable<MaterialDto>>(materials);
    }

    public async Task<IEnumerable<MaterialDto>> GetActiveMaterialsAsync()
    {
        var materials = await _unitOfWork.Materials.GetActiveMaterialsAsync();
        return _mapper.Map<IEnumerable<MaterialDto>>(materials);
    }

    public async Task<MaterialDto> UpdateMaterialAsync(MaterialDto materialDto)
    {
        try
        {
            var existingMaterial = await _unitOfWork.Materials.GetByIdAsync(materialDto.Id);
            if (existingMaterial == null)
            {
                throw new ArgumentException($"Material with ID {materialDto.Id} not found");
            }

            _mapper.Map(materialDto, existingMaterial);
            existingMaterial.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Materials.UpdateAsync(existingMaterial);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Updated material: {MaterialName} with ID: {MaterialId}", existingMaterial.Name, existingMaterial.Id);
            
            return _mapper.Map<MaterialDto>(existingMaterial);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating material with ID: {MaterialId}", materialDto.Id);
            throw;
        }
    }

    public async Task DeleteMaterialAsync(int id)
    {
        try
        {
            await _unitOfWork.Materials.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Deleted material with ID: {MaterialId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting material with ID: {MaterialId}", id);
            throw;
        }
    }

    public async Task<MaterialDto?> GetMaterialByCodeAsync(string code)
    {
        var material = await _unitOfWork.Materials.GetMaterialByCodeAsync(code);
        return material != null ? _mapper.Map<MaterialDto>(material) : null;
    }
}