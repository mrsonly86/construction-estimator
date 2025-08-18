using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ConstructionEstimator.Data.Context;
using ConstructionEstimator.Data.Repositories;
using ConstructionEstimator.Core.Interfaces;
using ConstructionEstimator.Services.Interfaces;
using ConstructionEstimator.Services.Implementations;
using ConstructionEstimator.Services.Mappings;
using ConstructionEstimator.Console.Services;

namespace ConstructionEstimator.Console;

class Program
{
    static async Task Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs/construction-estimator-.log", rollingInterval: RollingInterval.Day)
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            System.Console.WriteLine("=============================================================");
            System.Console.WriteLine("    CONSTRUCTION ESTIMATOR - Phần mềm dự toán xây dựng");
            System.Console.WriteLine("=============================================================");
            System.Console.WriteLine();

            // Build configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            // Build host
            var host = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services, configuration);
                })
                .Build();

            await host.StartAsync();

            // Ensure database is created and seeded
            using var scope = host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ConstructionEstimatorDbContext>();
            await dbContext.Database.EnsureCreatedAsync();

            System.Console.WriteLine("✓ Database initialized successfully");
            System.Console.WriteLine();

            // Run demo
            var demo = scope.ServiceProvider.GetRequiredService<DemoApplication>();
            await demo.RunDemoAsync();

            await host.StopAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application failed to start");
            System.Console.WriteLine($"ERROR: {ex.Message}");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ConstructionEstimatorDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                                 ?? "Data Source=ConstructionEstimator.db";
            options.UseSqlite(connectionString);
        });

        // Repositories and Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ICalculationService, CalculationService>();
        services.AddScoped<IMaterialService, MaterialService>();

        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // Demo Application
        services.AddTransient<DemoApplication>();
    }
}