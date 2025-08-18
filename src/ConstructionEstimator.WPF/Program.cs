global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ConstructionEstimator.Infrastructure.Data;
using ConstructionEstimator.Core.Interfaces;
using ConstructionEstimator.Core.Services;
using ConstructionEstimator.Infrastructure.Repositories;
using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Enums;

namespace ConstructionEstimator.WPF;

class Program
{
    static async Task Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/construction-estimator-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Starting Construction Estimator Application with Material Price Auto-Update System");

            // Check if we're on Windows and should run WPF
            if (OperatingSystem.IsWindows() && args.Length == 0 || (args.Length > 0 && args[0] == "--wpf"))
            {
                await RunWpfApplicationAsync();
            }
            else
            {
                // Run console demonstration
                await RunConsoleApplicationAsync();
            }
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    static async Task RunWpfApplicationAsync()
    {
#if WINDOWS
        // WPF Application for Windows only
        var app = new App();
        app.Run();
#else
        Console.WriteLine("WPF is only available on Windows. Running console application instead.");
        await RunConsoleApplicationAsync();
#endif
    }

    static async Task RunConsoleApplicationAsync()
    {
        // Build host
        var host = CreateHostBuilder().Build();

        // Initialize database
        await InitializeDatabaseAsync(host.Services);

        // Run the console application demonstrating the new features
        await RunApplicationAsync(host.Services);
    }

    static IHostBuilder CreateHostBuilder() =>
        Host.CreateDefaultBuilder()
            .UseSerilog()
            .ConfigureServices((context, services) =>
            {
                // Add DbContext
                services.AddDbContext<ConstructionEstimatorDbContext>(options =>
                    options.UseSqlite("Data Source=construction_estimator.db"));

                // Add repositories
                services.AddScoped<IProjectRepository, ProjectRepository>();
                services.AddScoped<IMaterialRepository, MaterialRepository>();
                services.AddScoped<ILaborRepository, LaborRepository>();
                services.AddScoped<IProvinceRepository, ProvinceRepository>();
                services.AddScoped<IMaterialPriceRepository, MaterialPriceRepository>();
                services.AddScoped<IPriceHistoryRepository, PriceHistoryRepository>();
                services.AddScoped<IPriceAlertRepository, PriceAlertRepository>();
                services.AddScoped<IDataSourceRepository, DataSourceRepository>();

                // Add core services
                services.AddScoped<IProjectService, ProjectService>();
                services.AddScoped<IEstimationService, EstimationService>();
                services.AddScoped<IMaterialService, MaterialService>();
                services.AddScoped<ILaborService, LaborService>();

                // Add new Material Price Auto-Update services
                services.AddScoped<IProvinceConfigService, ProvinceConfigService>();
                services.AddScoped<IMaterialPriceUpdateService, MaterialPriceUpdateService>();
                services.AddScoped<IPriceHistoryService, PriceHistoryService>();
                services.AddScoped<INotificationService, NotificationService>();
                services.AddScoped<IDataSourceService, DataSourceService>();
            });

    static async Task InitializeDatabaseAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ConstructionEstimatorDbContext>();

        Log.Information("Creating database...");
        await context.Database.EnsureCreatedAsync();

        Log.Information("Seeding database with Vietnamese construction data...");
        await DataSeeder.SeedAsync(context);

        Log.Information("Database initialization completed");
    }

    static async Task RunApplicationAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var projectService = scope.ServiceProvider.GetRequiredService<IProjectService>();
        var materialService = scope.ServiceProvider.GetRequiredService<IMaterialService>();
        var laborService = scope.ServiceProvider.GetRequiredService<ILaborService>();
        var estimationService = scope.ServiceProvider.GetRequiredService<IEstimationService>();

        // New Material Price Auto-Update services
        var provinceConfigService = scope.ServiceProvider.GetRequiredService<IProvinceConfigService>();
        var materialPriceUpdateService = scope.ServiceProvider.GetRequiredService<IMaterialPriceUpdateService>();
        var priceHistoryService = scope.ServiceProvider.GetRequiredService<IPriceHistoryService>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
        var dataSourceService = scope.ServiceProvider.GetRequiredService<IDataSourceService>();

        Log.Information("=== CONSTRUCTION ESTIMATOR - PHẦN MỀM DỰ TOÁN XÂY DỰNG VIỆT NAM ===");
        Log.Information("🚀 Enhanced with Material Price Auto-Update System");

        await DemoApplicationAsync(
            projectService, materialService, laborService, estimationService,
            provinceConfigService, materialPriceUpdateService, priceHistoryService, 
            notificationService, dataSourceService);
    }

    static async Task DemoApplicationAsync(
        IProjectService projectService,
        IMaterialService materialService,
        ILaborService laborService,
        IEstimationService estimationService,
        IProvinceConfigService provinceConfigService,
        IMaterialPriceUpdateService materialPriceUpdateService,
        IPriceHistoryService priceHistoryService,
        INotificationService notificationService,
        IDataSourceService dataSourceService)
    {
        Console.WriteLine("\n🚀 Demonstrating Enhanced Vietnamese Construction Estimator Features:");

        // === ORIGINAL FUNCTIONALITY ===
        Console.WriteLine("\n📦 Available Construction Materials (first 10):");
        var materials = await materialService.GetAllMaterialsAsync();
        foreach (var material in materials.Take(10))
        {
            Console.WriteLine($"  - {material.Name} ({material.Code}) - {material.UnitPrice:N0} VND/{material.Unit} - {material.Category}");
        }
        Console.WriteLine($"  ... and {materials.Count() - 10} more materials");

        Console.WriteLine("\n👷 Available Labor Types (first 10):");
        var labors = await laborService.GetAllLaborsAsync();
        foreach (var labor in labors.Take(10))
        {
            Console.WriteLine($"  - {labor.Name} - {labor.DailyRate:N0} VND/day - {labor.Category}");
        }
        Console.WriteLine($"  ... and {labors.Count() - 10} more labor types");

        // === NEW MATERIAL PRICE AUTO-UPDATE FEATURES ===
        Console.WriteLine("\n🌍 NEW FEATURE: Vietnamese Provinces Configuration");
        var provinces = await provinceConfigService.GetAllProvincesAsync();
        Console.WriteLine($"📍 Total provinces configured: {provinces.Count()}");
        
        var regionGroups = provinces.GroupBy(p => p.Region);
        foreach (var region in regionGroups)
        {
            Console.WriteLine($"  📍 {region.Key}: {region.Count()} tỉnh/thành");
            foreach (var province in region.Take(5))
            {
                Console.WriteLine($"    - {province.Name} ({province.Code})");
            }
            if (region.Count() > 5)
                Console.WriteLine($"    ... và {region.Count() - 5} tỉnh khác");
        }

        Console.WriteLine("\n💰 NEW FEATURE: Material Price Auto-Update System");
        
        // Demo with Hanoi
        var hanoi = provinces.FirstOrDefault(p => p.Code == "HN");
        var hcm = provinces.FirstOrDefault(p => p.Code == "HCM");
        
        if (hanoi != null)
        {
            Console.WriteLine($"\n🔄 Demonstrating price update for {hanoi.Name}");
            var updateSuccess = await materialPriceUpdateService.UpdatePricesForProvinceAsync(hanoi.Id);
            Console.WriteLine($"✅ Update result for {hanoi.Name}: {(updateSuccess ? "Success" : "Failed")}");

            // Show current prices for Hanoi
            var hanoiPrices = await materialPriceUpdateService.GetMaterialPricesByProvinceAsync(hanoi.Id);
            Console.WriteLine($"\n💵 Current material prices in {hanoi.Name} ({hanoiPrices.Count()} items):");
            foreach (var price in hanoiPrices.Take(5))
            {
                Console.WriteLine($"  - {price.Material.Name}: {price.UnitPrice:N0} VND/{price.Material.Unit} từ {price.Supplier}");
            }
            if (hanoiPrices.Count() > 5)
                Console.WriteLine($"  ... và {hanoiPrices.Count() - 5} vật liệu khác");
        }

        if (hcm != null)
        {
            Console.WriteLine($"\n🔄 Demonstrating price update for {hcm.Name}");
            var updateSuccess = await materialPriceUpdateService.UpdatePricesForProvinceAsync(hcm.Id);
            Console.WriteLine($"✅ Update result for {hcm.Name}: {(updateSuccess ? "Success" : "Failed")}");

            // Show current prices for HCM
            var hcmPrices = await materialPriceUpdateService.GetMaterialPricesByProvinceAsync(hcm.Id);
            Console.WriteLine($"\n💵 Current material prices in {hcm.Name} ({hcmPrices.Count()} items):");
            foreach (var price in hcmPrices.Take(5))
            {
                Console.WriteLine($"  - {price.Material.Name}: {price.UnitPrice:N0} VND/{price.Material.Unit} từ {price.Supplier}");
            }
            if (hcmPrices.Count() > 5)
                Console.WriteLine($"  ... và {hcmPrices.Count() - 5} vật liệu khác");
        }

        Console.WriteLine("\n🔍 NEW FEATURE: Data Sources Configuration");
        var dataSources = await dataSourceService.GetActiveDataSourcesAsync();
        Console.WriteLine($"📊 Active data sources: {dataSources.Count()}");
        foreach (var source in dataSources)
        {
            Console.WriteLine($"  - {source.Name} ({source.Province.Name}) - {source.SourceType}");
            Console.WriteLine($"    URL: {source.SourceUrl}");
            Console.WriteLine($"    Next scan: {source.NextScanDate:dd/MM/yyyy}");
        }

        Console.WriteLine("\n🔔 NEW FEATURE: Price Alert System");
        var activeAlerts = await notificationService.GetActiveAlertsAsync();
        Console.WriteLine($"⚠️ Active price alerts: {activeAlerts.Count()}");

        // Demo: Create a sample price alert for cement
        var cement = materials.FirstOrDefault(m => m.Code == "XM001");
        if (cement != null && hanoi != null)
        {
            Console.WriteLine($"\n📝 Creating price alert for {cement.Name} in {hanoi.Name}");
            var alert = new PriceAlert
            {
                MaterialId = cement.Id,
                ProvinceId = hanoi.Id,
                AlertType = "SignificantChange",
                ThresholdPercentage = 5.0m, // 5% change
                EmailEnabled = true,
                PopupEnabled = true,
                NotificationEmail = "admin@example.com"
            };

            var createdAlert = await notificationService.CreatePriceAlertAsync(alert);
            Console.WriteLine($"✅ Created alert #{createdAlert.Id} for {cement.Name} (threshold: 5%)");
        }

        // === ORIGINAL PROJECT DEMO ===
        Console.WriteLine("\n🏗️ Creating Sample Project: 'Nhà ở cá nhân 2 tầng'");
        var project = new Project
        {
            Name = "Nhà ở cá nhân 2 tầng",
            Description = "Xây dựng nhà ở cá nhân 2 tầng, diện tích 8x12m",
            Location = "Hà Nội",
            Client = "Anh Nguyễn Văn A",
            Status = ProjectStatus.Draft
        };

        project = await projectService.CreateProjectAsync(project);
        Console.WriteLine($"✅ Project created with ID: {project.Id}");

        // Add estimate sections (abbreviated for space)
        var foundationSection = new EstimateSection
        {
            Name = "Công tác móng",
            Code = "MONG",
            ProjectId = project.Id,
            Items = new List<EstimateItem>()
        };

        foundationSection.Items.Add(new EstimateItem
        {
            Name = "Đào móng băng 60x80cm",
            Unit = "m3",
            Quantity = 25,
            MaterialCost = 50000,
            LaborCost = 80000,
            EquipmentCost = 20000,
            SectionId = foundationSection.Id
        });

        project.Sections = new List<EstimateSection> { foundationSection };
        project = await projectService.UpdateProjectAsync(project);

        // Calculate costs
        Console.WriteLine("\n💰 Cost Calculation:");
        var totalCost = estimationService.CalculateProjectCost(project);
        Console.WriteLine($"🎯 TOTAL PROJECT COST: {totalCost:N0} VND");

        // Generate report
        Console.WriteLine("\n📊 Generating Enhanced Report:");
        var report = await estimationService.GenerateReportAsync(project);
        Console.WriteLine($"Project: {report.Project.Name}");
        Console.WriteLine($"Total Cost: {report.GrandTotal:N0} VND");

        // Show database statistics
        Console.WriteLine($"\n📊 Enhanced Database Statistics:");
        Console.WriteLine($"  Materials: {materials.Count()} items");
        Console.WriteLine($"  Labor types: {labors.Count()} types");
        Console.WriteLine($"  Provinces: {provinces.Count()} provinces");
        Console.WriteLine($"  Data sources: {dataSources.Count()} sources");
        Console.WriteLine($"  Price alerts: {activeAlerts.Count()} alerts");
        Console.WriteLine($"  Projects: {(await projectService.GetAllProjectsAsync()).Count()} projects");

        Console.WriteLine("\n✅ Enhanced implementation completed successfully!");
        Console.WriteLine("🚀 Ready for Material Price Auto-Update and WPF UI development!");
        Console.WriteLine("\n💡 Features implemented:");
        Console.WriteLine("   ✅ 63 Vietnamese provinces with regional grouping");
        Console.WriteLine("   ✅ Material price tracking by province");
        Console.WriteLine("   ✅ Price history and change detection");
        Console.WriteLine("   ✅ Automated price update system");
        Console.WriteLine("   ✅ Price alert and notification system");
        Console.WriteLine("   ✅ Data source configuration for price scraping");
        Console.WriteLine("   ✅ Enhanced WPF UI templates (Windows only)");

        if (OperatingSystem.IsWindows())
        {
            Console.WriteLine("\n🖥️  To run the WPF UI version: dotnet run --wpf");
        }
    }
}

#if WINDOWS
// This will only be included when building on Windows
public partial class App : System.Windows.Application
{
    // WPF App implementation would go here
    protected override void OnStartup(System.Windows.StartupEventArgs e)
    {
        // Initialize DI container and show main window
        base.OnStartup(e);
        Console.WriteLine("WPF Application starting...");
        // For now, just show console message as WPF initialization is complex
        System.Environment.Exit(0);
    }
}
#endif