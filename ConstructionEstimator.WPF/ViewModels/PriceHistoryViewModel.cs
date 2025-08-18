using System.Collections.ObjectModel;
using ConstructionEstimator.Core.Models;
using ConstructionEstimator.Core.Services;

namespace ConstructionEstimator.WPF.ViewModels;

public class PriceHistoryViewModel : ViewModelBase
{
    private readonly IPriceHistoryService _priceHistoryService;
    private string _selectedMaterial = string.Empty;
    private string? _selectedProvince;

    public PriceHistoryViewModel(IPriceHistoryService priceHistoryService)
    {
        _priceHistoryService = priceHistoryService;
        PriceHistory = new ObservableCollection<MaterialPrice>();
    }

    public ObservableCollection<MaterialPrice> PriceHistory { get; }

    public string SelectedMaterial
    {
        get => _selectedMaterial;
        set
        {
            if (SetProperty(ref _selectedMaterial, value))
            {
                _ = Task.Run(LoadPriceHistoryAsync);
            }
        }
    }

    public string? SelectedProvince
    {
        get => _selectedProvince;
        set
        {
            if (SetProperty(ref _selectedProvince, value))
            {
                _ = Task.Run(LoadPriceHistoryAsync);
            }
        }
    }

    private async Task LoadPriceHistoryAsync()
    {
        if (string.IsNullOrEmpty(SelectedMaterial))
            return;

        var history = await _priceHistoryService.GetPriceHistoryAsync(SelectedMaterial, SelectedProvince);
        
        PriceHistory.Clear();
        foreach (var price in history)
        {
            PriceHistory.Add(price);
        }
    }

    public async Task<IEnumerable<MaterialPrice>> GetRecentChangesAsync(int days = 30)
    {
        return await _priceHistoryService.GetRecentChangesAsync(days);
    }
}