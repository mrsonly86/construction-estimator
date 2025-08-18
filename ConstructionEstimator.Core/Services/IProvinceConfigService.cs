namespace ConstructionEstimator.Core.Services;

public interface IProvinceConfigService
{
    IEnumerable<string> GetProvinces();
    string GetDataSourceUrl(string province);
    bool IsProvinceSupported(string province);
    Task<Dictionary<string, string>> GetProvinceConfigsAsync();
}