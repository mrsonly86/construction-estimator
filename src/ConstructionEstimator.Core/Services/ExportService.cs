using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Interfaces;
using OfficeOpenXml;

namespace ConstructionEstimator.Core.Services;

public class ExportService : IExportService
{
    public async Task<byte[]> ExportToExcelAsync(Project project)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Dự toán xây dựng");

        // Header information
        worksheet.Cells["A1"].Value = "DỰ TOÁN XÂY DỰNG";
        worksheet.Cells["A1:I1"].Merge = true;
        worksheet.Cells["A1"].Style.Font.Size = 16;
        worksheet.Cells["A1"].Style.Font.Bold = true;

        // Project information
        int row = 3;
        worksheet.Cells[$"A{row}"].Value = "Tên dự án:";
        worksheet.Cells[$"B{row}"].Value = project.Name;
        worksheet.Cells[$"A{row}"].Style.Font.Bold = true;
        
        row++;
        worksheet.Cells[$"A{row}"].Value = "Khách hàng:";
        worksheet.Cells[$"B{row}"].Value = project.Client;
        worksheet.Cells[$"A{row}"].Style.Font.Bold = true;
        
        row++;
        worksheet.Cells[$"A{row}"].Value = "Địa điểm:";
        worksheet.Cells[$"B{row}"].Value = project.Location;
        worksheet.Cells[$"A{row}"].Style.Font.Bold = true;
        
        row++;
        worksheet.Cells[$"A{row}"].Value = "Ngày tạo:";
        worksheet.Cells[$"B{row}"].Value = project.CreatedDate.ToString("dd/MM/yyyy");
        worksheet.Cells[$"A{row}"].Style.Font.Bold = true;

        // Headers
        row += 2;
        var headers = new string[]
        {
            "STT", "Tên công việc", "Đơn vị", "Khối lượng", 
            "Vật liệu (VND)", "Nhân công (VND)", "Máy móc (VND)", 
            "Đơn giá (VND)", "Thành tiền (VND)"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cells[row, i + 1];
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
        }

        // Data rows
        int itemNumber = 1;
        row++;

        foreach (var section in project.Sections)
        {
            // Section header
            worksheet.Cells[$"A{row}"].Value = section.Name;
            worksheet.Cells[$"A{row}:I{row}"].Merge = true;
            worksheet.Cells[$"A{row}"].Style.Font.Bold = true;
            row++;

            foreach (var item in section.Items)
            {
                worksheet.Cells[$"A{row}"].Value = itemNumber++;
                worksheet.Cells[$"B{row}"].Value = item.Name;
                worksheet.Cells[$"C{row}"].Value = item.Unit;
                worksheet.Cells[$"D{row}"].Value = item.Quantity;
                worksheet.Cells[$"E{row}"].Value = item.MaterialCost;
                worksheet.Cells[$"F{row}"].Value = item.LaborCost;
                worksheet.Cells[$"G{row}"].Value = item.EquipmentCost;
                worksheet.Cells[$"H{row}"].Value = item.UnitPrice;
                worksheet.Cells[$"I{row}"].Value = item.TotalCost;

                // Format numbers
                worksheet.Cells[$"D{row}"].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[$"E{row}:I{row}"].Style.Numberformat.Format = "#,##0";

                row++;
            }

            // Section total
            worksheet.Cells[$"H{row}"].Value = "Tổng phần:";
            worksheet.Cells[$"H{row}"].Style.Font.Bold = true;
            worksheet.Cells[$"I{row}"].Value = section.TotalCost;
            worksheet.Cells[$"I{row}"].Style.Font.Bold = true;
            worksheet.Cells[$"I{row}"].Style.Numberformat.Format = "#,##0";
            row++;
        }

        // Grand total
        row++;
        worksheet.Cells[$"H{row}"].Value = "TỔNG CỘNG:";
        worksheet.Cells[$"H{row}"].Style.Font.Bold = true;
        worksheet.Cells[$"H{row}"].Style.Font.Size = 12;
        worksheet.Cells[$"I{row}"].Value = project.Sections.Sum(s => s.TotalCost);
        worksheet.Cells[$"I{row}"].Style.Font.Bold = true;
        worksheet.Cells[$"I{row}"].Style.Font.Size = 12;
        worksheet.Cells[$"I{row}"].Style.Numberformat.Format = "#,##0";

        // Auto-fit columns
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        return await Task.FromResult(package.GetAsByteArray());
    }

    public async Task<byte[]> ExportToPdfAsync(Project project)
    {
        // PDF export would require additional libraries like iTextSharp or similar
        // For now, return placeholder
        await Task.CompletedTask;
        throw new NotImplementedException("PDF export functionality requires additional PDF libraries");
    }
}