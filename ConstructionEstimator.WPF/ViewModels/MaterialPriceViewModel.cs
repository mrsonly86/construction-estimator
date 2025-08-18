using System.Collections.ObjectModel;
using ConstructionEstimator.Core.Models;
using ConstructionEstimator.WPF.Services;

namespace ConstructionEstimator.WPF.ViewModels;

public class MaterialPriceViewModel : ViewModelBase
{
    private readonly MaterialPriceDataProvider _dataProvider;
    private string? _selectedProvince;
    private bool _isLoading;

    public MaterialPriceViewModel(MaterialPriceDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
        MaterialPrices = new ObservableCollection<MaterialPrice>();
        SupportedProvinces = new ObservableCollection<string>(_dataProvider.GetSupportedProvinces());
        
        RefreshCommand = new RelayCommand(async () => await RefreshPricesAsync(), () => !IsLoading);
        LoadDataCommand = new RelayCommand(async () => await LoadDataAsync(), () => !IsLoading);
        
        // Load initial data
        _ = Task.Run(LoadDataAsync);
    }

    public ObservableCollection<MaterialPrice> MaterialPrices { get; }
    public ObservableCollection<string> SupportedProvinces { get; }

    public string? SelectedProvince
    {
        get => _selectedProvince;
        set
        {
            if (SetProperty(ref _selectedProvince, value))
            {
                _ = Task.Run(LoadDataAsync);
            }
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand RefreshCommand { get; }
    public ICommand LoadDataCommand { get; }

    private async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;
            var prices = await _dataProvider.GetMaterialPricesAsync(SelectedProvince);
            
            MaterialPrices.Clear();
            foreach (var price in prices)
            {
                MaterialPrices.Add(price);
            }
        }
        finally
        {
            IsLoading = false;
            CommandManager.InvalidateRequerySuggested();
        }
    }

    private async Task RefreshPricesAsync()
    {
        try
        {
            IsLoading = true;
            await _dataProvider.RefreshPricesAsync(SelectedProvince);
            await LoadDataAsync();
        }
        finally
        {
            IsLoading = false;
            CommandManager.InvalidateRequerySuggested();
        }
    }
}