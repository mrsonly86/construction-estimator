using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConstructionEstimator.Core.Services;

public class ProvinceConfigService : IProvinceConfigService
{
    private readonly IProvinceRepository _provinceRepository;
    private readonly ILogger<ProvinceConfigService> _logger;

    public ProvinceConfigService(IProvinceRepository provinceRepository, ILogger<ProvinceConfigService> logger)
    {
        _provinceRepository = provinceRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Province>> GetAllProvincesAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all provinces");
            return await _provinceRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all provinces");
            throw;
        }
    }

    public async Task<Province?> GetProvinceAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving province with ID: {ProvinceId}", id);
            return await _provinceRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving province with ID: {ProvinceId}", id);
            throw;
        }
    }

    public async Task<Province?> GetProvinceByCodeAsync(string code)
    {
        try
        {
            _logger.LogInformation("Retrieving province with code: {ProvinceCode}", code);
            return await _provinceRepository.GetProvinceByCodeAsync(code);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving province with code: {ProvinceCode}", code);
            throw;
        }
    }

    public async Task<Province> CreateProvinceAsync(Province province)
    {
        try
        {
            _logger.LogInformation("Creating new province: {ProvinceName}", province.Name);
            province.LastUpdated = DateTime.Now;
            
            var createdProvince = await _provinceRepository.AddAsync(province);
            await _provinceRepository.SaveChangesAsync();
            
            _logger.LogInformation("Successfully created province: {ProvinceName} with ID: {ProvinceId}", 
                province.Name, createdProvince.Id);
            return createdProvince;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating province: {ProvinceName}", province.Name);
            throw;
        }
    }

    public async Task<Province> UpdateProvinceAsync(Province province)
    {
        try
        {
            _logger.LogInformation("Updating province: {ProvinceName} (ID: {ProvinceId})", 
                province.Name, province.Id);
            
            province.LastUpdated = DateTime.Now;
            await _provinceRepository.UpdateAsync(province);
            await _provinceRepository.SaveChangesAsync();
            
            _logger.LogInformation("Successfully updated province: {ProvinceName}", province.Name);
            return province;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating province: {ProvinceName} (ID: {ProvinceId})", 
                province.Name, province.Id);
            throw;
        }
    }

    public async Task DeleteProvinceAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting province with ID: {ProvinceId}", id);
            await _provinceRepository.DeleteAsync(id);
            await _provinceRepository.SaveChangesAsync();
            _logger.LogInformation("Successfully deleted province with ID: {ProvinceId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting province with ID: {ProvinceId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Province>> GetProvincesByRegionAsync(string region)
    {
        try
        {
            _logger.LogInformation("Retrieving provinces for region: {Region}", region);
            return await _provinceRepository.GetProvincesByRegionAsync(region);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving provinces for region: {Region}", region);
            throw;
        }
    }
}