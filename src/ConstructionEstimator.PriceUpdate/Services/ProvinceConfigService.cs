using Microsoft.Extensions.Logging;
using ConstructionEstimator.Core.Interfaces;
using ConstructionEstimator.Core.Entities;

namespace ConstructionEstimator.PriceUpdate.Services;

public class ProvinceConfigService : IProvinceConfigService
{
    private readonly ILogger<ProvinceConfigService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly HttpClient _httpClient;

    public ProvinceConfigService(
        ILogger<ProvinceConfigService> logger,
        IUnitOfWork unitOfWork,
        HttpClient httpClient)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _httpClient = httpClient;
    }

    public async Task<ProvinceConfig?> GetConfigAsync(string provinceCode)
    {
        var configs = await _unitOfWork.ProvinceConfigs.GetAllAsync();
        return configs.FirstOrDefault(c => c.ProvinceCode == provinceCode);
    }

    public async Task<IEnumerable<ProvinceConfig>> GetAllConfigsAsync()
    {
        return await _unitOfWork.ProvinceConfigs.GetAllAsync();
    }

    public async Task<bool> UpdateConfigAsync(ProvinceConfig config)
    {
        config.LastModified = DateTime.UtcNow;
        
        var existingConfigs = await _unitOfWork.ProvinceConfigs.GetAllAsync();
        var existing = existingConfigs.FirstOrDefault(c => c.ProvinceCode == config.ProvinceCode);
        
        if (existing != null)
        {
            existing.DepartmentName = config.DepartmentName;
            existing.WebsiteUrl = config.WebsiteUrl;
            existing.PriceListUrl = config.PriceListUrl;
            existing.BackupUrl = config.BackupUrl;
            existing.UpdateScheduleDay = config.UpdateScheduleDay;
            existing.AutoUpdateEnabled = config.AutoUpdateEnabled;
            existing.LastUpdateStatus = config.LastUpdateStatus;
            existing.LastSuccessfulUpdate = config.LastSuccessfulUpdate;
            existing.PriceTableSelector = config.PriceTableSelector;
            existing.PdfDownloadSelector = config.PdfDownloadSelector;
            existing.ExcelDownloadSelector = config.ExcelDownloadSelector;
            existing.LastModified = config.LastModified;
            
            await _unitOfWork.ProvinceConfigs.UpdateAsync(existing);
        }
        else
        {
            await _unitOfWork.ProvinceConfigs.AddAsync(config);
        }
        
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> TestConnectionAsync(string provinceCode)
    {
        try
        {
            var config = await GetConfigAsync(provinceCode);
            if (config?.WebsiteUrl == null)
                return false;

            var response = await _httpClient.GetAsync(config.WebsiteUrl);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error testing connection for province {ProvinceCode}", provinceCode);
            return false;
        }
    }

    public async Task<IEnumerable<Province>> GetSupportedProvincesAsync()
    {
        return await _unitOfWork.Provinces.GetAllAsync();
    }
}