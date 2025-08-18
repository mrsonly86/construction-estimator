using ConstructionEstimator.Core.Models;
using ConstructionEstimator.Core.Services;

namespace ConstructionEstimator.WPF.Services;

public class MaterialPriceDataProvider
{
    private readonly IMaterialPriceUpdateService _materialPriceUpdateService;
    private readonly IProvinceConfigService _provinceConfigService;

    public MaterialPriceDataProvider(
        IMaterialPriceUpdateService materialPriceUpdateService,
        IProvinceConfigService provinceConfigService)
    {
        _materialPriceUpdateService = materialPriceUpdateService;
        _provinceConfigService = provinceConfigService;
    }

    public async Task<IEnumerable<MaterialPrice>> GetMaterialPricesAsync(string? province = null)
    {
        return await _materialPriceUpdateService.GetMaterialPricesAsync(province);
    }

    public async Task<bool> RefreshPricesAsync(string? province = null)
    {
        if (string.IsNullOrEmpty(province))
        {
            await _materialPriceUpdateService.RefreshAllPricesAsync();
            return true;
        }
        else
        {
            return await _materialPriceUpdateService.UpdateMaterialPricesAsync(province);
        }
    }

    public IEnumerable<string> GetSupportedProvinces()
    {
        return _provinceConfigService.GetProvinces();
    }
}