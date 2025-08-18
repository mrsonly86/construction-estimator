using Microsoft.Extensions.Logging;
using Moq;
using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Interfaces;
using ConstructionEstimator.PriceUpdate.Services;

namespace ConstructionEstimator.Tests;

public class PriceHistoryServiceTests
{
    private readonly Mock<ILogger<PriceHistoryService>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<PriceHistory>> _mockPriceHistoryRepo;
    private readonly Mock<IRepository<MaterialPrice>> _mockMaterialPriceRepo;
    private readonly PriceHistoryService _service;

    public PriceHistoryServiceTests()
    {
        _mockLogger = new Mock<ILogger<PriceHistoryService>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPriceHistoryRepo = new Mock<IRepository<PriceHistory>>();
        _mockMaterialPriceRepo = new Mock<IRepository<MaterialPrice>>();
        
        _mockUnitOfWork.Setup(u => u.PriceHistories).Returns(_mockPriceHistoryRepo.Object);
        _mockUnitOfWork.Setup(u => u.MaterialPrices).Returns(_mockMaterialPriceRepo.Object);
        
        _service = new PriceHistoryService(_mockLogger.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task LogPriceChangeAsync_Should_Calculate_Correct_Percentage()
    {
        // Arrange
        var materialId = 1;
        var provinceCode = "01";
        var oldPrice = 2000000m;
        var newPrice = 2500000m;
        var source = "Test";

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _service.LogPriceChangeAsync(materialId, provinceCode, oldPrice, newPrice, source);

        // Assert
        Assert.True(result);
        _mockPriceHistoryRepo.Verify(r => r.AddAsync(It.Is<PriceHistory>(ph => 
            ph.ChangePercentage == 25.0m && 
            ph.ChangeType == PriceChangeType.Increase)), Times.Once);
    }

    [Fact]
    public async Task GetPriceTrendAsync_Should_Return_Rising_For_Positive_Changes()
    {
        // Arrange
        var materialId = 1;
        var provinceCode = "01";
        var cutoffDate = DateTime.UtcNow.AddDays(-90);
        var histories = new List<PriceHistory>
        {
            new() { MaterialId = 1, ProvinceCode = "01", ChangePercentage = 5m, NewPrice = 2100000m, ChangeDate = DateTime.UtcNow.AddDays(-10) },
            new() { MaterialId = 1, ProvinceCode = "01", ChangePercentage = 3m, NewPrice = 2200000m, ChangeDate = DateTime.UtcNow.AddDays(-5) },
            new() { MaterialId = 1, ProvinceCode = "01", ChangePercentage = 2m, NewPrice = 2300000m, ChangeDate = DateTime.UtcNow.AddDays(-1) }
        };

        _mockPriceHistoryRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(histories);

        // Act
        var trend = await _service.GetPriceTrendAsync(materialId, provinceCode, 90);

        // Assert
        Assert.Equal(TrendDirection.Rising, trend.Direction);
        Assert.Equal(10m, trend.ChangePercentage); // 5 + 3 + 2
        Assert.True(trend.AveragePrice > 0);
    }

    [Fact]
    public async Task GetAveragePriceAsync_Should_Return_Current_Price_When_No_History()
    {
        // Arrange
        var materialId = 1;
        var provinceCode = "01";
        var currentPrice = new MaterialPrice { MaterialId = 1, ProvinceCode = "01", Price = 2500000m, IsActive = true };

        _mockPriceHistoryRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<PriceHistory>());
        _mockMaterialPriceRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<MaterialPrice> { currentPrice });

        // Act
        var averagePrice = await _service.GetAveragePriceAsync(materialId, provinceCode, 30);

        // Assert
        Assert.Equal(2500000m, averagePrice);
    }
}