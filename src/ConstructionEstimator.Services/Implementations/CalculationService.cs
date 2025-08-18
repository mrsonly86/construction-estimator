using ConstructionEstimator.Services.Interfaces;

namespace ConstructionEstimator.Services.Implementations;

public class CalculationService : ICalculationService
{
    public decimal CalculateItemTotal(decimal quantity, decimal unitPrice)
    {
        return quantity * unitPrice;
    }

    public decimal CalculateWithContingency(decimal amount, decimal contingencyPercentage)
    {
        return amount * (1 + contingencyPercentage / 100);
    }

    public decimal CalculateWithProfit(decimal amount, decimal profitPercentage)
    {
        return amount * (1 + profitPercentage / 100);
    }

    public decimal CalculateOverhead(decimal laborCost, decimal overheadPercentage)
    {
        return laborCost * (overheadPercentage / 100);
    }

    public Dictionary<string, decimal> CalculateCostBreakdown(
        decimal materialCost, 
        decimal laborCost, 
        decimal equipmentCost, 
        decimal profitPercentage, 
        decimal contingencyPercentage)
    {
        var subtotal = materialCost + laborCost + equipmentCost;
        var overheadCost = CalculateOverhead(laborCost, 15); // 15% overhead default
        var costBeforeProfit = subtotal + overheadCost;
        var profitAmount = costBeforeProfit * (profitPercentage / 100);
        var costBeforeContingency = costBeforeProfit + profitAmount;
        var contingencyAmount = costBeforeContingency * (contingencyPercentage / 100);
        var totalCost = costBeforeContingency + contingencyAmount;

        return new Dictionary<string, decimal>
        {
            ["MaterialCost"] = materialCost,
            ["LaborCost"] = laborCost,
            ["EquipmentCost"] = equipmentCost,
            ["OverheadCost"] = overheadCost,
            ["Subtotal"] = subtotal,
            ["ProfitAmount"] = profitAmount,
            ["ContingencyAmount"] = contingencyAmount,
            ["TotalCost"] = totalCost
        };
    }
}