using Microsoft.EntityFrameworkCore;
using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Data.Context;
using ConstructionEstimator.Data.Repositories;

namespace ConstructionEstimator.Tests;

public class DatabaseTests
{
    private ConstructionEstimatorDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ConstructionEstimatorDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ConstructionEstimatorDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task Can_Create_Project()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);
        
        var project = new Project
        {
            Name = "Test Project",
            Description = "Test Description",
            Location = "Test Location",
            ProvinceCode = "01",
            CreatedDate = DateTime.UtcNow,
            EstimatedBudget = 1000000,
            Status = ProjectStatus.Planning
        };

        // Act
        await unitOfWork.Projects.AddAsync(project);
        await unitOfWork.SaveChangesAsync();

        // Assert
        var savedProject = await unitOfWork.Projects.GetByIdAsync(project.Id);
        Assert.NotNull(savedProject);
        Assert.Equal("Test Project", savedProject.Name);
        Assert.Equal("01", savedProject.ProvinceCode);
    }

    [Fact]
    public async Task Can_Get_All_Materials()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);

        // Act
        var materials = await unitOfWork.Materials.GetAllAsync();

        // Assert
        Assert.NotEmpty(materials);
        Assert.Contains(materials, m => m.Code == "BT_B25");
        Assert.Contains(materials, m => m.Code == "THEP_CB300V");
    }

    [Fact]
    public async Task Can_Get_All_Provinces()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);

        // Act
        var provinces = await unitOfWork.Provinces.GetAllAsync();

        // Assert
        Assert.NotEmpty(provinces);
        Assert.Contains(provinces, p => p.Code == "01" && p.Name == "Hà Nội");
        Assert.Contains(provinces, p => p.Code == "79" && p.Name == "TP. Hồ Chí Minh");
    }

    [Fact]
    public async Task Can_Create_Material_Price()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);

        var materialPrice = new MaterialPrice
        {
            MaterialId = 1, // BT_B25
            ProvinceCode = "01", // Hà Nội
            Price = 2500000, // 2.5M VND per m3
            EffectiveDate = DateTime.UtcNow,
            Source = "Test Source",
            LastUpdated = DateTime.UtcNow,
            IsActive = true
        };

        // Act
        await unitOfWork.MaterialPrices.AddAsync(materialPrice);
        await unitOfWork.SaveChangesAsync();

        // Assert
        var savedPrice = await unitOfWork.MaterialPrices.GetByIdAsync(materialPrice.Id);
        Assert.NotNull(savedPrice);
        Assert.Equal(2500000, savedPrice.Price);
        Assert.Equal("01", savedPrice.ProvinceCode);
    }

    [Fact]
    public async Task Can_Create_Price_History()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var unitOfWork = new UnitOfWork(context);

        var priceHistory = new PriceHistory
        {
            MaterialId = 1,
            ProvinceCode = "01",
            OldPrice = 2000000,
            NewPrice = 2500000,
            ChangePercentage = 25.0m,
            ChangeDate = DateTime.UtcNow,
            Source = "Test Update",
            ChangeType = PriceChangeType.Increase
        };

        // Act
        await unitOfWork.PriceHistories.AddAsync(priceHistory);
        await unitOfWork.SaveChangesAsync();

        // Assert
        var savedHistory = await unitOfWork.PriceHistories.GetByIdAsync(priceHistory.Id);
        Assert.NotNull(savedHistory);
        Assert.Equal(25.0m, savedHistory.ChangePercentage);
        Assert.Equal(PriceChangeType.Increase, savedHistory.ChangeType);
    }
}