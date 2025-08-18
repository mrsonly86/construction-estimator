using OfficeOpenXml;
using ConstructionEstimator.Core.Models;
using ConstructionEstimator.Core.Enums;
using ConstructionEstimator.Reports.Interfaces;

namespace ConstructionEstimator.Reports.Implementations;

public class ExcelExporter : IExcelExporter
{
    public async Task<byte[]> ExportProjectToExcelAsync(ProjectDto project)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        using var package = new ExcelPackage();
        
        // Project Info Sheet
        var projectSheet = package.Workbook.Worksheets.Add("Thông tin dự án");
        await CreateProjectInfoSheet(projectSheet, project);
        
        // Estimate Items Sheet
        var estimateSheet = package.Workbook.Worksheets.Add("Bảng dự toán");
        await CreateEstimateSheet(estimateSheet, project);
        
        // Cost Summary Sheet
        var summarySheet = package.Workbook.Worksheets.Add("Tổng hợp chi phí");
        await CreateSummarySheet(summarySheet, project);
        
        return package.GetAsByteArray();
    }

    public async Task<byte[]> ExportMaterialListToExcelAsync(IEnumerable<MaterialDto> materials)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Danh sách vật liệu");
        
        // Headers
        worksheet.Cells[1, 1].Value = "Mã VL";
        worksheet.Cells[1, 2].Value = "Tên vật liệu";
        worksheet.Cells[1, 3].Value = "Loại";
        worksheet.Cells[1, 4].Value = "Đơn vị";
        worksheet.Cells[1, 5].Value = "Đơn giá";
        worksheet.Cells[1, 6].Value = "Nhà cung cấp";
        worksheet.Cells[1, 7].Value = "Thương hiệu";
        
        // Style headers
        using var headerRange = worksheet.Cells[1, 1, 1, 7];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
        
        // Data
        int row = 2;
        foreach (var material in materials)
        {
            worksheet.Cells[row, 1].Value = material.Code;
            worksheet.Cells[row, 2].Value = material.Name;
            worksheet.Cells[row, 3].Value = material.Category.ToString();
            worksheet.Cells[row, 4].Value = material.UnitOfMeasure.ToString();
            worksheet.Cells[row, 5].Value = material.UnitPrice;
            worksheet.Cells[row, 6].Value = material.Supplier;
            worksheet.Cells[row, 7].Value = material.Brand;
            
            // Format currency
            worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0";
            
            row++;
        }
        
        worksheet.Cells.AutoFitColumns();
        
        return await Task.FromResult(package.GetAsByteArray());
    }

    private async Task CreateProjectInfoSheet(ExcelWorksheet worksheet, ProjectDto project)
    {
        worksheet.Cells[1, 1].Value = "THÔNG TIN DỰ ÁN";
        worksheet.Cells[1, 1].Style.Font.Bold = true;
        worksheet.Cells[1, 1].Style.Font.Size = 16;
        
        worksheet.Cells[3, 1].Value = "Tên dự án:";
        worksheet.Cells[3, 2].Value = project.Name;
        
        worksheet.Cells[4, 1].Value = "Khách hàng:";
        worksheet.Cells[4, 2].Value = project.ClientName;
        
        worksheet.Cells[5, 1].Value = "Địa điểm:";
        worksheet.Cells[5, 2].Value = project.Location;
        
        worksheet.Cells[6, 1].Value = "Ngày bắt đầu:";
        worksheet.Cells[6, 2].Value = project.StartDate.ToString("dd/MM/yyyy");
        
        worksheet.Cells[7, 1].Value = "Trạng thái:";
        worksheet.Cells[7, 2].Value = project.Status.ToString();
        
        worksheet.Cells[8, 1].Value = "Tiền tệ:";
        worksheet.Cells[8, 2].Value = project.Currency;
        
        // Style
        using var labelRange = worksheet.Cells[3, 1, 8, 1];
        labelRange.Style.Font.Bold = true;
        
        worksheet.Cells.AutoFitColumns();
        await Task.CompletedTask;
    }

    private async Task CreateEstimateSheet(ExcelWorksheet worksheet, ProjectDto project)
    {
        worksheet.Cells[1, 1].Value = "BẢNG DỰ TOÁN";
        worksheet.Cells[1, 1].Style.Font.Bold = true;
        worksheet.Cells[1, 1].Style.Font.Size = 16;
        
        // Headers
        int headerRow = 3;
        worksheet.Cells[headerRow, 1].Value = "STT";
        worksheet.Cells[headerRow, 2].Value = "Mã";
        worksheet.Cells[headerRow, 3].Value = "Tên hạng mục";
        worksheet.Cells[headerRow, 4].Value = "Loại";
        worksheet.Cells[headerRow, 5].Value = "Đơn vị";
        worksheet.Cells[headerRow, 6].Value = "Số lượng";
        worksheet.Cells[headerRow, 7].Value = "Đơn giá";
        worksheet.Cells[headerRow, 8].Value = "Thành tiền";
        worksheet.Cells[headerRow, 9].Value = "Ghi chú";
        
        // Style headers
        using var headerRange = worksheet.Cells[headerRow, 1, headerRow, 9];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
        
        // Data
        int row = headerRow + 1;
        int stt = 1;
        foreach (var item in project.EstimateItems.OrderBy(i => i.SortOrder))
        {
            worksheet.Cells[row, 1].Value = stt;
            worksheet.Cells[row, 2].Value = item.Code;
            worksheet.Cells[row, 3].Value = item.Name;
            worksheet.Cells[row, 4].Value = item.Type.ToString();
            worksheet.Cells[row, 5].Value = item.UnitOfMeasure.ToString();
            worksheet.Cells[row, 6].Value = item.Quantity;
            worksheet.Cells[row, 7].Value = item.UnitPrice;
            worksheet.Cells[row, 8].Value = item.TotalPrice;
            worksheet.Cells[row, 9].Value = item.Notes;
            
            // Format numbers
            worksheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
            worksheet.Cells[row, 7].Style.Numberformat.Format = "#,##0";
            worksheet.Cells[row, 8].Style.Numberformat.Format = "#,##0";
            
            row++;
            stt++;
        }
        
        // Total row
        worksheet.Cells[row, 7].Value = "TỔNG CỘNG:";
        worksheet.Cells[row, 7].Style.Font.Bold = true;
        worksheet.Cells[row, 8].Value = project.EstimateItems.Sum(i => i.TotalPrice);
        worksheet.Cells[row, 8].Style.Font.Bold = true;
        worksheet.Cells[row, 8].Style.Numberformat.Format = "#,##0";
        
        worksheet.Cells.AutoFitColumns();
        await Task.CompletedTask;
    }

    private async Task CreateSummarySheet(ExcelWorksheet worksheet, ProjectDto project)
    {
        worksheet.Cells[1, 1].Value = "TỔNG HỢP CHI PHÍ";
        worksheet.Cells[1, 1].Style.Font.Bold = true;
        worksheet.Cells[1, 1].Style.Font.Size = 16;
        
        var materialCost = project.EstimateItems.Where(i => i.Type == EstimateItemType.Material).Sum(i => i.TotalPrice);
        var laborCost = project.EstimateItems.Where(i => i.Type == EstimateItemType.Labor).Sum(i => i.TotalPrice);
        var equipmentCost = project.EstimateItems.Where(i => i.Type == EstimateItemType.Equipment).Sum(i => i.TotalPrice);
        var subtotal = materialCost + laborCost + equipmentCost;
        var overhead = subtotal * 0.15m; // 15% overhead
        var profit = (subtotal + overhead) * (project.ProfitPercentage / 100);
        var contingency = (subtotal + overhead + profit) * (project.ContingencyPercentage / 100);
        var total = subtotal + overhead + profit + contingency;
        
        int row = 3;
        worksheet.Cells[row, 1].Value = "Chi phí vật liệu:";
        worksheet.Cells[row, 2].Value = materialCost;
        worksheet.Cells[row++, 2].Style.Numberformat.Format = "#,##0";
        
        worksheet.Cells[row, 1].Value = "Chi phí nhân công:";
        worksheet.Cells[row, 2].Value = laborCost;
        worksheet.Cells[row++, 2].Style.Numberformat.Format = "#,##0";
        
        worksheet.Cells[row, 1].Value = "Chi phí thiết bị:";
        worksheet.Cells[row, 2].Value = equipmentCost;
        worksheet.Cells[row++, 2].Style.Numberformat.Format = "#,##0";
        
        worksheet.Cells[row, 1].Value = "Tổng chi phí trực tiếp:";
        worksheet.Cells[row, 2].Value = subtotal;
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 2].Style.Font.Bold = true;
        worksheet.Cells[row++, 2].Style.Numberformat.Format = "#,##0";
        
        worksheet.Cells[row, 1].Value = "Chi phí quản lý (15%):";
        worksheet.Cells[row, 2].Value = overhead;
        worksheet.Cells[row++, 2].Style.Numberformat.Format = "#,##0";
        
        worksheet.Cells[row, 1].Value = $"Lợi nhuận ({project.ProfitPercentage}%):";
        worksheet.Cells[row, 2].Value = profit;
        worksheet.Cells[row++, 2].Style.Numberformat.Format = "#,##0";
        
        worksheet.Cells[row, 1].Value = $"Dự phòng ({project.ContingencyPercentage}%):";
        worksheet.Cells[row, 2].Value = contingency;
        worksheet.Cells[row++, 2].Style.Numberformat.Format = "#,##0";
        
        worksheet.Cells[row, 1].Value = "TỔNG GIÁ TRỊ DỰ ÁN:";
        worksheet.Cells[row, 2].Value = total;
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 1].Style.Font.Size = 14;
        worksheet.Cells[row, 2].Style.Font.Bold = true;
        worksheet.Cells[row, 2].Style.Font.Size = 14;
        worksheet.Cells[row, 2].Style.Numberformat.Format = "#,##0";
        worksheet.Cells[row, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        worksheet.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
        
        worksheet.Cells.AutoFitColumns();
        await Task.CompletedTask;
    }
}