using ConstructionEstimator.Core.Services;
using ConstructionEstimator.Infrastructure.Services;
using ConstructionEstimator.WPF.Services;
using ConstructionEstimator.WPF.ViewModels;

namespace ConstructionEstimator.WPF;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Construction Estimator - Material Price Management ===");
        Console.WriteLine();

        // Setup dependency injection
        var provinceConfigService = new ProvinceConfigService();
        var priceHistoryService = new PriceHistoryService();
        var notificationService = new NotificationService();
        var materialPriceUpdateService = new MaterialPriceUpdateService(
            provinceConfigService, priceHistoryService, notificationService);

        var dataProvider = new MaterialPriceDataProvider(materialPriceUpdateService, provinceConfigService);

        // Create ViewModels
        var materialPriceViewModel = new MaterialPriceViewModel(dataProvider);
        var priceHistoryViewModel = new PriceHistoryViewModel(priceHistoryService);
        var notificationViewModel = new NotificationViewModel(notificationService);
        var dashboardViewModel = new DashboardViewModel(
            materialPriceViewModel, priceHistoryViewModel, notificationViewModel, dataProvider);

        var app = new ConsoleApp(dashboardViewModel);
        await app.RunAsync();
    }
}

public class ConsoleApp
{
    private readonly DashboardViewModel _dashboardViewModel;
    private bool _running = true;

    public ConsoleApp(DashboardViewModel dashboardViewModel)
    {
        _dashboardViewModel = dashboardViewModel;
    }

    public async Task RunAsync()
    {
        Console.WriteLine("Đang tải dữ liệu ban đầu...");
        await Task.Delay(2000); // Give time for initial data loading

        while (_running)
        {
            ShowMainMenu();
            var choice = Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine();

            switch (char.ToUpper(choice.KeyChar))
            {
                case '1':
                    await ShowMaterialPricesAsync();
                    break;
                case '2':
                    await RefreshPricesAsync();
                    break;
                case '3':
                    await ShowPriceHistoryAsync();
                    break;
                case '4':
                    ShowNotifications();
                    break;
                case '5':
                    ShowDashboardSummary();
                    break;
                case 'Q':
                    _running = false;
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ!");
                    break;
            }

            if (_running)
            {
                Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        Console.WriteLine("Cảm ơn bạn đã sử dụng Construction Estimator!");
    }

    private void ShowMainMenu()
    {
        Console.WriteLine("=== MENU CHÍNH ===");
        Console.WriteLine("1. Xem bảng giá vật liệu");
        Console.WriteLine("2. Cập nhật giá vật liệu");
        Console.WriteLine("3. Xem lịch sử giá");
        Console.WriteLine("4. Xem thông báo");
        Console.WriteLine("5. Tổng quan dashboard");
        Console.WriteLine("Q. Thoát");
        Console.WriteLine();
        Console.Write("Chọn chức năng (1-5, Q): ");
    }

    private async Task ShowMaterialPricesAsync()
    {
        Console.WriteLine("=== BẢNG GIÁ VẬT LIỆU ===");
        
        var provinces = _dashboardViewModel.MaterialPriceViewModel.SupportedProvinces.Take(5).ToList();
        Console.WriteLine("\nCác tỉnh/thành có sẵn:");
        for (int i = 0; i < provinces.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {provinces[i]}");
        }
        Console.WriteLine("0. Tất cả tỉnh/thành");
        
        Console.Write("\nChọn tỉnh/thành (0-5): ");
        var choice = Console.ReadKey();
        Console.WriteLine();
        
        string? selectedProvince = null;
        if (choice.KeyChar >= '1' && choice.KeyChar <= '5')
        {
            var index = int.Parse(choice.KeyChar.ToString()) - 1;
            if (index < provinces.Count)
            {
                selectedProvince = provinces[index];
                _dashboardViewModel.MaterialPriceViewModel.SelectedProvince = selectedProvince;
            }
        }
        else if (choice.KeyChar == '0')
        {
            _dashboardViewModel.MaterialPriceViewModel.SelectedProvince = null;
        }

        await Task.Delay(500); // Give time for data loading

        Console.WriteLine($"\n--- Giá vật liệu {(selectedProvince ?? "tất cả tỉnh/thành")} ---");
        Console.WriteLine($"{"Vật liệu",-20} {"Tỉnh/Thành",-15} {"Giá",-15} {"Đơn vị",-8} {"Cập nhật",-12}");
        Console.WriteLine(new string('-', 80));

        var prices = _dashboardViewModel.MaterialPriceViewModel.MaterialPrices.Take(20);
        foreach (var price in prices)
        {
            Console.WriteLine($"{price.MaterialName,-20} {price.Province,-15} {price.Price,-15:N0} {price.Unit,-8} {price.LastUpdated:dd/MM HH:mm}");
        }

        if (!prices.Any())
        {
            Console.WriteLine("Không có dữ liệu.");
        }
    }

    private async Task RefreshPricesAsync()
    {
        Console.WriteLine("=== CẬP NHẬT GIÁ VẬT LIỆU ===");
        
        var provinces = _dashboardViewModel.MaterialPriceViewModel.SupportedProvinces.Take(5).ToList();
        Console.WriteLine("\nChọn tỉnh/thành để cập nhật:");
        for (int i = 0; i < provinces.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {provinces[i]}");
        }
        Console.WriteLine("0. Cập nhật tất cả");
        
        Console.Write("\nChọn tỉnh/thành (0-5): ");
        var choice = Console.ReadKey();
        Console.WriteLine();
        
        Console.WriteLine("\nĐang cập nhật giá vật liệu...");
        
        if (choice.KeyChar >= '1' && choice.KeyChar <= '5')
        {
            var index = int.Parse(choice.KeyChar.ToString()) - 1;
            if (index < provinces.Count)
            {
                var province = provinces[index];
                await _dashboardViewModel.DataProvider.RefreshPricesAsync(province);
                Console.WriteLine($"Đã cập nhật giá cho {province}");
            }
        }
        else if (choice.KeyChar == '0')
        {
            if (_dashboardViewModel.RefreshAllCommand.CanExecute(null))
            {
                _dashboardViewModel.RefreshAllCommand.Execute(null);
            }
            Console.WriteLine("Đã cập nhật giá cho tất cả tỉnh/thành");
        }
        else
        {
            Console.WriteLine("Lựa chọn không hợp lệ!");
        }
    }

    private async Task ShowPriceHistoryAsync()
    {
        Console.WriteLine("=== LỊCH SỬ BIẾN ĐỘNG GIÁ ===");
        
        var recentChanges = await _dashboardViewModel.PriceHistoryViewModel.GetRecentChangesAsync(30);
        
        Console.WriteLine("\nBiến động giá 30 ngày gần đây:");
        Console.WriteLine($"{"Vật liệu",-20} {"Tỉnh/Thành",-15} {"Giá",-15} {"Ngày cập nhật",-20}");
        Console.WriteLine(new string('-', 80));

        foreach (var change in recentChanges.Take(15))
        {
            Console.WriteLine($"{change.MaterialName,-20} {change.Province,-15} {change.Price,-15:N0} {change.LastUpdated:dd/MM/yyyy HH:mm:ss}");
        }

        if (!recentChanges.Any())
        {
            Console.WriteLine("Chưa có lịch sử biến động giá.");
        }
    }

    private void ShowNotifications()
    {
        Console.WriteLine("=== THÔNG BÁO HỆ THỐNG ===");
        
        var notifications = _dashboardViewModel.NotificationViewModel.Notifications.Take(10);
        
        if (notifications.Any())
        {
            foreach (var notification in notifications)
            {
                Console.WriteLine($"[{notification.Timestamp:HH:mm:ss}] {notification.Category}: {notification.Message}");
            }
        }
        else
        {
            Console.WriteLine("Không có thông báo mới.");
        }
    }

    private void ShowDashboardSummary()
    {
        Console.WriteLine("=== TỔNG QUAN HỆ THỐNG ===");
        Console.WriteLine();
        Console.WriteLine(_dashboardViewModel.GetSummaryInfo());
        Console.WriteLine();
        
        var materialCounts = _dashboardViewModel.MaterialPriceViewModel.MaterialPrices
            .GroupBy(p => p.Province)
            .Select(g => new { Province = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5);

        Console.WriteLine("Top 5 tỉnh/thành theo số lượng vật liệu:");
        foreach (var item in materialCounts)
        {
            Console.WriteLine($"  {item.Province}: {item.Count} vật liệu");
        }

        Console.WriteLine();
        var recentNotifications = _dashboardViewModel.NotificationViewModel.Notifications.Take(3);
        Console.WriteLine("Thông báo gần đây:");
        foreach (var notification in recentNotifications)
        {
            Console.WriteLine($"  {notification.DisplayText}");
        }
        
        if (!recentNotifications.Any())
        {
            Console.WriteLine("  Không có thông báo mới.");
        }
    }
}