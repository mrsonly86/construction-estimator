using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Services;
using ConstructionEstimator.Core.Enums;

namespace ConstructionEstimator.Tests;

public class FoundationTests
{
    [Fact]
    public void Project_CalculatesTotalCost_Correctly()
    {
        // Arrange
        var project = new Project
        {
            Name = "Test Project",
            Sections = new List<EstimateSection>
            {
                new EstimateSection
                {
                    Name = "Test Section",
                    Items = new List<EstimateItem>
                    {
                        new EstimateItem
                        {
                            Name = "Test Item 1",
                            Quantity = 10,
                            MaterialCost = 1000,
                            LaborCost = 500,
                            EquipmentCost = 200
                        },
                        new EstimateItem
                        {
                            Name = "Test Item 2",
                            Quantity = 5,
                            MaterialCost = 2000,
                            LaborCost = 800,
                            EquipmentCost = 300
                        }
                    }
                }
            }
        };

        var estimationService = new EstimationService();

        // Act
        var totalCost = estimationService.CalculateProjectCost(project);

        // Assert
        // Item 1: 10 * (1000 + 500 + 200) = 17,000
        // Item 2: 5 * (2000 + 800 + 300) = 15,500
        // Total: 32,500
        Assert.Equal(32500m, totalCost);
    }

    [Fact]
    public void EstimateItem_CalculatesUnitPrice_Correctly()
    {
        // Arrange
        var item = new EstimateItem
        {
            MaterialCost = 1000,
            LaborCost = 500,
            EquipmentCost = 200
        };

        // Act & Assert
        Assert.Equal(1700m, item.UnitPrice);
    }

    [Fact]
    public void EstimateItem_CalculatesTotalCost_Correctly()
    {
        // Arrange
        var item = new EstimateItem
        {
            Quantity = 10,
            MaterialCost = 1000,
            LaborCost = 500,
            EquipmentCost = 200
        };

        // Act & Assert
        Assert.Equal(17000m, item.TotalCost);
    }

    [Fact]
    public void EstimateSection_CalculatesTotalCost_Correctly()
    {
        // Arrange
        var section = new EstimateSection
        {
            Items = new List<EstimateItem>
            {
                new EstimateItem
                {
                    Quantity = 10,
                    MaterialCost = 100,
                    LaborCost = 50,
                    EquipmentCost = 20
                },
                new EstimateItem
                {
                    Quantity = 5,
                    MaterialCost = 200,
                    LaborCost = 80,
                    EquipmentCost = 30
                }
            }
        };

        // Act & Assert
        // Item 1: 10 * (100 + 50 + 20) = 1,700
        // Item 2: 5 * (200 + 80 + 30) = 1,550
        // Total: 3,250
        Assert.Equal(3250m, section.TotalCost);
    }

    [Theory]
    [InlineData(ProjectStatus.Draft)]
    [InlineData(ProjectStatus.InProgress)]
    [InlineData(ProjectStatus.Completed)]
    [InlineData(ProjectStatus.Cancelled)]
    [InlineData(ProjectStatus.OnHold)]
    public void ProjectStatus_AllEnumValues_Work(ProjectStatus status)
    {
        // Arrange
        var project = new Project
        {
            Status = status
        };

        // Act & Assert
        Assert.Equal(status, project.Status);
    }

    [Theory]
    [InlineData(LaborSkillLevel.Apprentice)]
    [InlineData(LaborSkillLevel.Skilled)]
    [InlineData(LaborSkillLevel.Expert)]
    [InlineData(LaborSkillLevel.Supervisor)]
    public void LaborSkillLevel_AllEnumValues_Work(LaborSkillLevel skillLevel)
    {
        // Arrange
        var labor = new Labor
        {
            SkillLevel = skillLevel
        };

        // Act & Assert
        Assert.Equal(skillLevel, labor.SkillLevel);
    }

    [Fact]
    public async Task EstimationService_GeneratesReport_WithCorrectData()
    {
        // Arrange
        var project = new Project
        {
            Name = "Test Project",
            Client = "Test Client",
            Location = "Test Location",
            Sections = new List<EstimateSection>
            {
                new EstimateSection
                {
                    Name = "Foundation",
                    Code = "FND",
                    Items = new List<EstimateItem>
                    {
                        new EstimateItem
                        {
                            Name = "Concrete",
                            Quantity = 10,
                            MaterialCost = 1000,
                            LaborCost = 500,
                            EquipmentCost = 200
                        }
                    }
                }
            }
        };

        var estimationService = new EstimationService();

        // Act
        var report = await estimationService.GenerateReportAsync(project);

        // Assert
        Assert.Equal(project, report.Project);
        Assert.Equal(10000m, report.TotalMaterialCost); // 10 * 1000
        Assert.Equal(5000m, report.TotalLaborCost); // 10 * 500
        Assert.Equal(2000m, report.TotalEquipmentCost); // 10 * 200
        Assert.Equal(17000m, report.GrandTotal);
        Assert.Single(report.SectionSummaries);
        Assert.Equal("Foundation", report.SectionSummaries[0].SectionName);
        Assert.Equal("FND", report.SectionSummaries[0].SectionCode);
        Assert.Equal(17000m, report.SectionSummaries[0].SectionTotal);
        Assert.Equal(1, report.SectionSummaries[0].ItemCount);
    }
}