using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ConstructionEstimator.Data.Context;
using ConstructionEstimator.Data.Repositories;
using ConstructionEstimator.Core.Interfaces;
using ConstructionEstimator.Services.Interfaces;
using ConstructionEstimator.Services.Implementations;
using ConstructionEstimator.Services.Mappings;
using ConstructionEstimator.Core.Models;
using ConstructionEstimator.Core.Enums;

namespace ConstructionEstimator.Tests.Unit;

public class ConstructionEstimatorIntegrationTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly ConstructionEstimatorDbContext _context;

    public ConstructionEstimatorIntegrationTests()
    {
        var services = new ServiceCollection();
        
        // Use in-memory database for testing
        services.AddDbContext<ConstructionEstimatorDbContext>(options =>
        {
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
        });

        // Add services
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ICalculationService, CalculationService>();
        services.AddScoped<IMaterialService, MaterialService>();
        services.AddLogging();

        _serviceProvider = services.BuildServiceProvider();
        _context = _serviceProvider.GetRequiredService<ConstructionEstimatorDbContext>();
        
        // Ensure database is created
        _context.Database.EnsureCreated();
    }

    [Fact]
    public void ProjectService_CreateProject_ShouldCreateProjectSuccessfully()
    {
        // Arrange
        var projectService = _serviceProvider.GetRequiredService<IProjectService>();
        var projectDto = new ProjectDto
        {
            Name = "Test Project",
            ClientName = "Test Client",
            Location = "Test Location",
            StartDate = DateTime.Now,
            Currency = "VND",
            ProfitPercentage = 10,
            ContingencyPercentage = 5,
            Status = ProjectStatus.Draft
        };

        // Act
        var result = projectService.CreateProjectAsync(projectDto).Result;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("Test Project", result.Name);
        Assert.Equal("Test Client", result.ClientName);
        Assert.Equal("Test Location", result.Location);
        Assert.Equal(ProjectStatus.Draft, result.Status);
    }

    [Fact]
    public void CalculationService_CalculateCostBreakdown_ShouldCalculateCorrectly()
    {
        // Arrange
        var calculationService = _serviceProvider.GetRequiredService<ICalculationService>();
        decimal materialCost = 1000000;
        decimal laborCost = 500000;
        decimal equipmentCost = 200000;
        decimal profitPercentage = 10;
        decimal contingencyPercentage = 5;

        // Act
        var result = calculationService.CalculateCostBreakdown(
            materialCost, laborCost, equipmentCost, profitPercentage, contingencyPercentage);

        // Assert
        Assert.Equal(materialCost, result["MaterialCost"]);
        Assert.Equal(laborCost, result["LaborCost"]);
        Assert.Equal(equipmentCost, result["EquipmentCost"]);
        Assert.Equal(1700000, result["Subtotal"]); // 1000000 + 500000 + 200000
        Assert.Equal(75000, result["OverheadCost"]); // 500000 * 15%
        Assert.Equal(177500, result["ProfitAmount"]); // (1700000 + 75000) * 10%
        Assert.Equal(95225, result["ContingencyAmount"]); // (1700000 + 75000 + 177500) * 5%
        Assert.Equal(2047725, result["TotalCost"]); // All costs combined
    }

    [Fact]
    public async Task MaterialService_CreateMaterial_ShouldCreateMaterialSuccessfully()
    {
        // Arrange
        var materialService = _serviceProvider.GetRequiredService<IMaterialService>();
        var materialDto = new MaterialDto
        {
            Name = "Test Material",
            Code = "TEST001",
            Category = MaterialCategory.Concrete,
            UnitOfMeasure = UnitOfMeasure.CubicMeter,
            UnitPrice = 2500000,
            Supplier = "Test Supplier",
            IsActive = true
        };

        // Act
        var result = await materialService.CreateMaterialAsync(materialDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("Test Material", result.Name);
        Assert.Equal("TEST001", result.Code);
        Assert.Equal(MaterialCategory.Concrete, result.Category);
        Assert.Equal(2500000, result.UnitPrice);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task ProjectService_GetAllProjects_ShouldReturnAllProjects()
    {
        // Arrange
        var projectService = _serviceProvider.GetRequiredService<IProjectService>();
        
        // Create test projects
        await projectService.CreateProjectAsync(new ProjectDto { Name = "Project 1", ClientName = "Client 1", Location = "Location 1" });
        await projectService.CreateProjectAsync(new ProjectDto { Name = "Project 2", ClientName = "Client 2", Location = "Location 2" });

        // Act
        var result = await projectService.GetAllProjectsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, p => p.Name == "Project 1");
        Assert.Contains(result, p => p.Name == "Project 2");
    }

    [Fact]
    public async Task MaterialService_GetActiveMaterials_ShouldReturnOnlyActiveMaterials()
    {
        // Arrange
        var materialService = _serviceProvider.GetRequiredService<IMaterialService>();
        
        // Create test materials
        await materialService.CreateMaterialAsync(new MaterialDto 
        { 
            Name = "Active Material", 
            Code = "ACT001",
            Category = MaterialCategory.Concrete,
            UnitOfMeasure = UnitOfMeasure.Ton,
            UnitPrice = 1000000,
            IsActive = true 
        });
        
        await materialService.CreateMaterialAsync(new MaterialDto 
        { 
            Name = "Inactive Material", 
            Code = "INA001",
            Category = MaterialCategory.Steel,
            UnitOfMeasure = UnitOfMeasure.Ton,
            UnitPrice = 2000000,
            IsActive = false 
        });

        // Act
        var result = await materialService.GetActiveMaterialsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Active Material", result.First().Name);
        Assert.True(result.First().IsActive);
    }

    [Fact]
    public async Task CalculationService_CalculateItemTotal_ShouldCalculateCorrectly()
    {
        // Arrange
        var calculationService = _serviceProvider.GetRequiredService<ICalculationService>();

        // Act
        var result = calculationService.CalculateItemTotal(10.5m, 250000m);

        // Assert
        Assert.Equal(2625000m, result);
    }

    public void Dispose()
    {
        _context?.Dispose();
        _serviceProvider?.Dispose();
    }
}