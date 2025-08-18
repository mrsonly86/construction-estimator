using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ConstructionEstimator.Core.Interfaces;
using ConstructionEstimator.Data.Context;
using ConstructionEstimator.Data.Repositories;
using ConstructionEstimator.PriceUpdate.Services;

namespace ConstructionEstimator.WPF;

internal class Program
{
    public static async Task Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("logs/construction-estimator.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Console.WriteLine("🏗️  Construction Estimator - Vietnamese Material Price System");
            Console.WriteLine("================================================================");

            var host = CreateHostBuilder(args).Build();
            
            // Initialize database
            await InitializeDatabaseAsync(host);
            
            // Run the application
            await RunApplicationAsync(host);
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

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureServices((context, services) =>
            {
                // Database
                services.AddDbContext<ConstructionEstimatorDbContext>(options =>
                    options.UseInMemoryDatabase("ConstructionEstimatorDb"));

                // Repositories
                services.AddScoped<IUnitOfWork, UnitOfWork>();

                // Services
                services.AddScoped<IMaterialPriceUpdateService, MaterialPriceUpdateService>();
                services.AddScoped<IProvinceConfigService, ProvinceConfigService>();
                services.AddScoped<IPriceHistoryService, PriceHistoryService>();

                // HTTP Client
                services.AddHttpClient<MaterialPriceUpdateService>();
                services.AddHttpClient<ProvinceConfigService>();

                // Configuration
                services.Configure<PriceUpdateOptions>(options =>
                {
                    options.TimeoutSeconds = 30;
                    options.RetryCount = 3;
                    options.EnableBackupSources = true;
                });
            });

    private static async Task InitializeDatabaseAsync(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ConstructionEstimatorDbContext>();
        
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();
        
        Console.WriteLine("✅ Database initialized successfully");
    }

    private static async Task RunApplicationAsync(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var provinceConfigService = scope.ServiceProvider.GetRequiredService<IProvinceConfigService>();
        var priceUpdateService = scope.ServiceProvider.GetRequiredService<IMaterialPriceUpdateService>();

        logger.LogInformation("Starting Construction Estimator application");

        while (true)
        {
            Console.WriteLine("\n📋 MENU - Choose an option:");
            Console.WriteLine("1. View Materials");
            Console.WriteLine("2. View Provinces");
            Console.WriteLine("3. View Material Prices");
            Console.WriteLine("4. Setup Province Configuration");
            Console.WriteLine("5. Test Price Update");
            Console.WriteLine("6. View Price History");
            Console.WriteLine("7. Create Sample Project");
            Console.WriteLine("0. Exit");
            Console.Write("\nEnter your choice: ");

            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await ShowMaterialsAsync(unitOfWork);
                        break;
                    case "2":
                        await ShowProvincesAsync(unitOfWork);
                        break;
                    case "3":
                        await ShowMaterialPricesAsync(unitOfWork);
                        break;
                    case "4":
                        await SetupProvinceConfigAsync(provinceConfigService);
                        break;
                    case "5":
                        await TestPriceUpdateAsync(priceUpdateService);
                        break;
                    case "6":
                        await ShowPriceHistoryAsync(unitOfWork);
                        break;
                    case "7":
                        await CreateSampleProjectAsync(unitOfWork);
                        break;
                    case "0":
                        Console.WriteLine("👋 Goodbye!");
                        return;
                    default:
                        Console.WriteLine("❌ Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error executing menu option {Choice}", choice);
                Console.WriteLine($"❌ Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private static async Task ShowMaterialsAsync(IUnitOfWork unitOfWork)
    {
        Console.WriteLine("\n🧱 MATERIALS LIST");
        Console.WriteLine("================");
        
        var materials = await unitOfWork.Materials.GetAllAsync();
        foreach (var material in materials)
        {
            Console.WriteLine($"ID: {material.Id}");
            Console.WriteLine($"Code: {material.Code}");
            Console.WriteLine($"Name: {material.Name}");
            Console.WriteLine($"Unit: {material.Unit}");
            Console.WriteLine($"Category: {material.Category}");
            Console.WriteLine($"Active: {(material.IsActive ? "Yes" : "No")}");
            Console.WriteLine("---");
        }
    }

    private static async Task ShowProvincesAsync(IUnitOfWork unitOfWork)
    {
        Console.WriteLine("\n🏛️  PROVINCES LIST");
        Console.WriteLine("==================");
        
        var provinces = await unitOfWork.Provinces.GetAllAsync();
        foreach (var province in provinces)
        {
            Console.WriteLine($"Code: {province.Code}");
            Console.WriteLine($"Name: {province.Name}");
            Console.WriteLine($"Full Name: {province.FullName}");
            Console.WriteLine($"Region: {province.Region}");
            Console.WriteLine($"Active: {(province.IsActive ? "Yes" : "No")}");
            Console.WriteLine("---");
        }
    }

    private static async Task ShowMaterialPricesAsync(IUnitOfWork unitOfWork)
    {
        Console.WriteLine("\n💰 MATERIAL PRICES");
        Console.WriteLine("==================");
        
        var prices = await unitOfWork.MaterialPrices.GetAllAsync();
        if (!prices.Any())
        {
            Console.WriteLine("No material prices found. Set up province configurations and run price updates first.");
            return;
        }

        foreach (var price in prices.Where(p => p.IsActive))
        {
            Console.WriteLine($"Material ID: {price.MaterialId}");
            Console.WriteLine($"Province: {price.ProvinceCode}");
            Console.WriteLine($"Price: {price.Price:N0} VND");
            Console.WriteLine($"Effective Date: {price.EffectiveDate:yyyy-MM-dd}");
            Console.WriteLine($"Source: {price.Source}");
            Console.WriteLine($"Last Updated: {price.LastUpdated:yyyy-MM-dd HH:mm}");
            Console.WriteLine("---");
        }
    }

    private static async Task SetupProvinceConfigAsync(IProvinceConfigService provinceConfigService)
    {
        Console.WriteLine("\n⚙️  PROVINCE CONFIGURATION SETUP");
        Console.WriteLine("=================================");
        
        Console.Write("Enter province code (01-63): ");
        var provinceCode = Console.ReadLine();
        
        if (string.IsNullOrEmpty(provinceCode) || provinceCode.Length != 2)
        {
            Console.WriteLine("❌ Invalid province code");
            return;
        }

        Console.Write("Enter department name (e.g., 'Sở Xây dựng Hà Nội'): ");
        var departmentName = Console.ReadLine();
        
        Console.Write("Enter website URL (optional): ");
        var websiteUrl = Console.ReadLine();
        
        Console.Write("Enter price list URL (optional): ");
        var priceListUrl = Console.ReadLine();

        var config = new ConstructionEstimator.Core.Entities.ProvinceConfig
        {
            ProvinceCode = provinceCode,
            DepartmentName = departmentName ?? "",
            WebsiteUrl = websiteUrl,
            PriceListUrl = priceListUrl,
            AutoUpdateEnabled = true,
            UpdateScheduleDay = 15
        };

        var success = await provinceConfigService.UpdateConfigAsync(config);
        
        if (success)
        {
            Console.WriteLine("✅ Province configuration saved successfully");
        }
        else
        {
            Console.WriteLine("❌ Failed to save province configuration");
        }
    }

    private static async Task TestPriceUpdateAsync(IMaterialPriceUpdateService priceUpdateService)
    {
        Console.WriteLine("\n🔄 PRICE UPDATE TEST");
        Console.WriteLine("====================");
        
        Console.Write("Enter province code to test: ");
        var provinceCode = Console.ReadLine();
        
        if (string.IsNullOrEmpty(provinceCode))
        {
            Console.WriteLine("❌ Invalid province code");
            return;
        }

        Console.WriteLine($"Testing price update for province {provinceCode}...");
        
        var result = await priceUpdateService.UpdatePricesForProvinceAsync(provinceCode);
        
        if (result)
        {
            Console.WriteLine("✅ Price update test completed successfully");
            var status = await priceUpdateService.GetLastUpdateStatusAsync(provinceCode);
            Console.WriteLine($"Last update: {status.UpdateTime:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"Materials updated: {status.MaterialsUpdated}");
        }
        else
        {
            Console.WriteLine("❌ Price update test failed");
        }
    }

    private static async Task ShowPriceHistoryAsync(IUnitOfWork unitOfWork)
    {
        Console.WriteLine("\n📈 PRICE HISTORY");
        Console.WriteLine("================");
        
        var histories = await unitOfWork.PriceHistories.GetAllAsync();
        if (!histories.Any())
        {
            Console.WriteLine("No price history found.");
            return;
        }

        foreach (var history in histories.OrderByDescending(h => h.ChangeDate).Take(10))
        {
            Console.WriteLine($"Material ID: {history.MaterialId}");
            Console.WriteLine($"Province: {history.ProvinceCode}");
            Console.WriteLine($"Price Change: {history.OldPrice:N0} → {history.NewPrice:N0} VND");
            Console.WriteLine($"Change %: {history.ChangePercentage:F2}%");
            Console.WriteLine($"Date: {history.ChangeDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"Type: {history.ChangeType}");
            Console.WriteLine("---");
        }
    }

    private static async Task CreateSampleProjectAsync(IUnitOfWork unitOfWork)
    {
        Console.WriteLine("\n🏗️  CREATE SAMPLE PROJECT");
        Console.WriteLine("=========================");
        
        var project = new ConstructionEstimator.Core.Entities.Project
        {
            Name = "Nhà ở cá nhân 2 tầng",
            Description = "Xây dựng nhà ở cá nhân 2 tầng tại Hà Nội",
            Location = "Phường Cầu Giấy, Quận Cầu Giấy, Hà Nội",
            ProvinceCode = "01",
            CreatedDate = DateTime.UtcNow,
            EstimatedBudget = 2000000000, // 2 tỷ VND
            Status = ConstructionEstimator.Core.Entities.ProjectStatus.Planning
        };

        await unitOfWork.Projects.AddAsync(project);
        await unitOfWork.SaveChangesAsync();
        
        Console.WriteLine("✅ Sample project created successfully");
        Console.WriteLine($"Project ID: {project.Id}");
        Console.WriteLine($"Name: {project.Name}");
        Console.WriteLine($"Location: {project.Location}");
        Console.WriteLine($"Estimated Budget: {project.EstimatedBudget:N0} VND");
    }
}