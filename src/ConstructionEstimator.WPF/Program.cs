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
            Log.Information("Starting Construction Estimator Application");
            
            // Build host
            var host = CreateHostBuilder(args).Build();
            
            // Initialize database
            await InitializeDatabaseAsync(host.Services);
            
            // Run the application
            await RunApplicationAsync(host.Services);
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

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
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

                // Add services
                services.AddScoped<IProjectService, ProjectService>();
                services.AddScoped<IEstimationService, EstimationService>();
                services.AddScoped<IMaterialService, MaterialService>();
                services.AddScoped<ILaborService, LaborService>();
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

        Log.Information("=== CONSTRUCTION ESTIMATOR - PHẦN MỀM DỰ TOÁN XÂY DỰNG VIỆT NAM ===");
        
        // Demo functionality
        await DemoApplicationAsync(projectService, materialService, laborService, estimationService);
    }

    static async Task DemoApplicationAsync(
        IProjectService projectService,
        IMaterialService materialService,
        ILaborService laborService,
        IEstimationService estimationService)
    {
        Console.WriteLine("\n🚀 Demonstrating Vietnamese Construction Estimator Features:");
        
        // 1. Show available materials
        Console.WriteLine("\n📦 Available Construction Materials (first 10):");
        var materials = await materialService.GetAllMaterialsAsync();
        foreach (var material in materials.Take(10))
        {
            Console.WriteLine($"  - {material.Name} ({material.Code}) - {material.UnitPrice:N0} VND/{material.Unit} - {material.Category}");
        }
        Console.WriteLine($"  ... and {materials.Count() - 10} more materials");

        // 2. Show available labor
        Console.WriteLine("\n👷 Available Labor Types (first 10):");
        var labors = await laborService.GetAllLaborsAsync();
        foreach (var labor in labors.Take(10))
        {
            Console.WriteLine($"  - {labor.Name} - {labor.DailyRate:N0} VND/day - {labor.Category}");
        }
        Console.WriteLine($"  ... and {labors.Count() - 10} more labor types");

        // 3. Create sample project
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

        // 4. Add estimate sections
        Console.WriteLine("\n📋 Adding Estimate Sections:");
        var foundationSection = new EstimateSection
        {
            Name = "Công tác móng",
            Code = "MONG",
            ProjectId = project.Id,
            Items = new List<EstimateItem>()
        };

        var structureSection = new EstimateSection
        {
            Name = "Công tác kết cấu",
            Code = "KETCAU",
            ProjectId = project.Id,
            Items = new List<EstimateItem>()
        };

        project.Sections = new List<EstimateSection> { foundationSection, structureSection };

        // 5. Add estimate items to foundation section
        var cement = materials.First(m => m.Code == "XM001");
        var sand = materials.First(m => m.Code == "CT001");
        var stone = materials.First(m => m.Code == "DA001");
        var masonLabor = labors.First(l => l.Name.Contains("Thợ xây chính"));

        foundationSection.Items.Add(new EstimateItem
        {
            Name = "Đào móng băng 60x80cm",
            Unit = "m3",
            Quantity = 25,
            MaterialCost = 50000, // Cost per m3
            LaborCost = 80000,
            EquipmentCost = 20000,
            SectionId = foundationSection.Id
        });

        foundationSection.Items.Add(new EstimateItem
        {
            Name = "Đổ bê tông móng M150",
            Unit = "m3",
            Quantity = 15,
            MaterialCost = 1200000, // Cost per m3 (cement + sand + stone)
            LaborCost = 200000,
            EquipmentCost = 100000,
            SectionId = foundationSection.Id
        });

        // 6. Add estimate items to structure section
        var steel = materials.First(m => m.Code == "CB012");
        var steelWorker = labors.First(l => l.Name.Contains("Thợ sắt chính"));

        structureSection.Items.Add(new EstimateItem
        {
            Name = "Cốt thép cột phi 12",
            Unit = "kg",
            Quantity = 1200,
            MaterialCost = steel.UnitPrice,
            LaborCost = 3000, // VND per kg
            EquipmentCost = 500,
            SectionId = structureSection.Id
        });

        structureSection.Items.Add(new EstimateItem
        {
            Name = "Đổ bê tông cột M200",
            Unit = "m3",
            Quantity = 8,
            MaterialCost = 1400000,
            LaborCost = 250000,
            EquipmentCost = 150000,
            SectionId = structureSection.Id
        });

        project = await projectService.UpdateProjectAsync(project);

        // 7. Calculate costs
        Console.WriteLine("\n💰 Cost Calculation:");
        foreach (var section in project.Sections)
        {
            var sectionCost = estimationService.CalculateSectionCost(section);
            Console.WriteLine($"  {section.Name}: {sectionCost:N0} VND");
            
            foreach (var item in section.Items)
            {
                var itemCost = estimationService.CalculateItemCost(item);
                Console.WriteLine($"    - {item.Name}: {item.Quantity} {item.Unit} x {item.UnitPrice:N0} = {itemCost:N0} VND");
            }
        }

        var totalCost = estimationService.CalculateProjectCost(project);
        Console.WriteLine($"\n🎯 TOTAL PROJECT COST: {totalCost:N0} VND");

        // 8. Generate report
        Console.WriteLine("\n📊 Generating Estimate Report:");
        var report = await estimationService.GenerateReportAsync(project);
        
        Console.WriteLine($"Project: {report.Project.Name}");
        Console.WriteLine($"Client: {report.Project.Client}");
        Console.WriteLine($"Location: {report.Project.Location}");
        Console.WriteLine($"Generated: {report.GeneratedDate:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"\nCost Breakdown:");
        Console.WriteLine($"  Material Cost:  {report.TotalMaterialCost:N0} VND");
        Console.WriteLine($"  Labor Cost:     {report.TotalLaborCost:N0} VND");
        Console.WriteLine($"  Equipment Cost: {report.TotalEquipmentCost:N0} VND");
        Console.WriteLine($"  GRAND TOTAL:    {report.GrandTotal:N0} VND");

        Console.WriteLine($"\nSection Summary:");
        foreach (var summary in report.SectionSummaries)
        {
            Console.WriteLine($"  {summary.SectionName} ({summary.SectionCode}): {summary.SectionTotal:N0} VND ({summary.ItemCount} items)");
        }

        // 9. Show project status
        Console.WriteLine($"\n📈 Project Status: {project.Status}");
        Console.WriteLine($"Created: {project.CreatedDate:dd/MM/yyyy HH:mm}");
        
        Console.WriteLine("\n✅ Foundation implementation completed successfully!");
        Console.WriteLine("🚀 Ready for immediate use and further development!");
        
        // Show database statistics
        Console.WriteLine($"\n📊 Database Statistics:");
        Console.WriteLine($"  Materials: {materials.Count()} items");
        Console.WriteLine($"  Labor types: {labors.Count()} types");
        Console.WriteLine($"  Projects: {(await projectService.GetAllProjectsAsync()).Count()} projects");
    }
}