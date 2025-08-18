using ConstructionEstimator.WPF.ViewModels;
using ConstructionEstimator.WPF.Services;

namespace ConstructionEstimator.WPF.ViewModels;

public class DashboardViewModel : ViewModelBase
{
    private readonly MaterialPriceDataProvider _dataProvider;

    public DashboardViewModel(
        MaterialPriceViewModel materialPriceViewModel,
        PriceHistoryViewModel priceHistoryViewModel,
        NotificationViewModel notificationViewModel,
        MaterialPriceDataProvider dataProvider)
    {
        MaterialPriceViewModel = materialPriceViewModel;
        PriceHistoryViewModel = priceHistoryViewModel;
        NotificationViewModel = notificationViewModel;
        _dataProvider = dataProvider;

        RefreshAllCommand = new RelayCommand(async () => await RefreshAllDataAsync());
    }

    public MaterialPriceViewModel MaterialPriceViewModel { get; }
    public PriceHistoryViewModel PriceHistoryViewModel { get; }
    public NotificationViewModel NotificationViewModel { get; }
    public MaterialPriceDataProvider DataProvider => _dataProvider;

    public ICommand RefreshAllCommand { get; }

    private async Task RefreshAllDataAsync()
    {
        await _dataProvider.RefreshPricesAsync();
    }

    public string GetSummaryInfo()
    {
        var totalMaterials = MaterialPriceViewModel.MaterialPrices.Count;
        var provinces = MaterialPriceViewModel.MaterialPrices.Select(p => p.Province).Distinct().Count();
        var latestUpdate = MaterialPriceViewModel.MaterialPrices
            .Select(p => p.LastUpdated)
            .DefaultIfEmpty()
            .Max();

        return $"Tổng số vật liệu: {totalMaterials} | " +
               $"Số tỉnh/thành: {provinces} | " +
               $"Cập nhật gần nhất: {(latestUpdate == default ? "Chưa có" : latestUpdate.ToString("dd/MM/yyyy HH:mm"))}";
    }
}