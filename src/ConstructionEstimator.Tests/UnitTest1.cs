using ConstructionEstimator.Core.Models;
using ConstructionEstimator.Core.Services;
using Xunit;

namespace ConstructionEstimator.Tests;

public class BasicFunctionalityTests
{
    [Fact]
    public void Project_Creation_SetsDefaultValues()
    {
        // Arrange & Act
        var project = new Project
        {
            Name = "Test Project",
            Location = "Hanoi",
            Client = "Test Client",
            Contractor = "Test Contractor"
        };

        // Assert
        Assert.Equal("Test Project", project.Name);
        Assert.Equal("Hanoi", project.Location);
        Assert.Equal(ProjectStatus.Draft, project.Status);
        Assert.Equal(10, project.VatRate);
        Assert.Equal(15, project.ProfitMargin);
        Assert.Equal(8, project.GeneralCostsRate);
    }

    [Fact]
    public void Material_Creation_SetsCorrectProperties()
    {
        // Arrange & Act
        var material = new Material
        {
            Code = "CEM001",
            Name = "Xi măng PC40",
            Unit = "tấn",
            Category = MaterialCategory.Cement,
            CurrentPrice = 2800000
        };

        // Assert
        Assert.Equal("CEM001", material.Code);
        Assert.Equal("Xi măng PC40", material.Name);
        Assert.Equal("tấn", material.Unit);
        Assert.Equal(MaterialCategory.Cement, material.Category);
        Assert.Equal(2800000, material.CurrentPrice);
        Assert.True(material.IsActive);
    }

    [Fact]
    public void EstimateItem_TotalAmount_CalculatedCorrectly()
    {
        // Arrange
        var estimateItem = new EstimateItem
        {
            Code = "ITEM001",
            Name = "Test Item",
            Unit = "m3",
            Quantity = 10.5m,
            UnitPrice = 1500000
        };

        // Act
        var totalAmount = estimateItem.TotalAmount;

        // Assert
        Assert.Equal(15750000, totalAmount);
    }

    [Fact]
    public async Task ProjectService_CreateProject_ReturnsProject()
    {
        // Arrange
        var service = new ProjectService();
        var project = new Project
        {
            Name = "Test Project",
            Location = "HCMC",
            Client = "Test Client",
            Contractor = "Test Contractor"
        };

        // Act
        var result = await service.CreateProjectAsync(project);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(project.Name, result.Name);
        Assert.Equal(project.Location, result.Location);
        Assert.True(result.Id > 0); // Service should assign an ID
    }

    [Fact]
    public async Task MaterialService_CreateMaterial_ReturnsMaterial()
    {
        // Arrange
        var service = new MaterialService();
        var material = new Material
        {
            Code = "TEST001",
            Name = "Test Material",
            Unit = "kg",
            Category = MaterialCategory.Other,
            CurrentPrice = 50000
        };

        // Act
        var result = await service.CreateMaterialAsync(material);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(material.Code, result.Code);
        Assert.Equal(material.Name, result.Name);
        Assert.True(result.Id > 0);
    }
}