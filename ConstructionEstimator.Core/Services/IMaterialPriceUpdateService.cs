using ConstructionEstimator.Core.Models;

namespace ConstructionEstimator.Core.Services;

public interface IMaterialPriceUpdateService
{
    Task<IEnumerable<MaterialPrice>> GetMaterialPricesAsync(string? province = null);
    Task<bool> UpdateMaterialPricesAsync(string province);
    Task<IEnumerable<MaterialPrice>> RefreshAllPricesAsync();
    event EventHandler<MaterialPriceUpdatedEventArgs>? PriceUpdated;
}

public class MaterialPriceUpdatedEventArgs : EventArgs
{
    public MaterialPrice UpdatedPrice { get; }
    public string ChangeType { get; }

    public MaterialPriceUpdatedEventArgs(MaterialPrice updatedPrice, string changeType)
    {
        UpdatedPrice = updatedPrice;
        ChangeType = changeType;
    }
}