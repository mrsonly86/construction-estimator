using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Interfaces;

namespace ConstructionEstimator.Core.Services;

public class EstimationService : IEstimationService
{
    public decimal CalculateItemCost(EstimateItem item)
    {
        return item.TotalCost;
    }

    public decimal CalculateSectionCost(EstimateSection section)
    {
        return section.TotalCost;
    }

    public decimal CalculateProjectCost(Project project)
    {
        return project.Sections?.Sum(s => s.TotalCost) ?? 0;
    }

    public async Task<EstimateReport> GenerateReportAsync(Project project)
    {
        var report = new EstimateReport
        {
            Project = project,
            GeneratedDate = DateTime.Now
        };

        decimal totalMaterial = 0;
        decimal totalLabor = 0;
        decimal totalEquipment = 0;

        foreach (var section in project.Sections)
        {
            var sectionSummary = new SectionSummary
            {
                SectionName = section.Name,
                SectionCode = section.Code,
                SectionTotal = section.TotalCost,
                ItemCount = section.Items.Count
            };

            report.SectionSummaries.Add(sectionSummary);

            foreach (var item in section.Items)
            {
                totalMaterial += item.MaterialCost * item.Quantity;
                totalLabor += item.LaborCost * item.Quantity;
                totalEquipment += item.EquipmentCost * item.Quantity;
            }
        }

        report.TotalMaterialCost = totalMaterial;
        report.TotalLaborCost = totalLabor;
        report.TotalEquipmentCost = totalEquipment;
        report.GrandTotal = totalMaterial + totalLabor + totalEquipment;

        return await Task.FromResult(report);
    }
}