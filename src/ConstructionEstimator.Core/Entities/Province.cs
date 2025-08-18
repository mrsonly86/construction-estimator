namespace ConstructionEstimator.Core.Entities;

public class Province
{
    public string Code { get; set; } = string.Empty; // Mã tỉnh/thành (01-63)
    public string Name { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string Region { get; set; } = string.Empty; // Miền: Bắc, Trung, Nam
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual ICollection<MaterialPrice> MaterialPrices { get; set; } = new List<MaterialPrice>();
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    public virtual ProvinceConfig? ProvinceConfig { get; set; }
}

public class ProvinceConfig
{
    public int Id { get; set; }
    public string ProvinceCode { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty; // Tên Sở Xây dựng
    public string? WebsiteUrl { get; set; }
    public string? PriceListUrl { get; set; }
    public string? BackupUrl { get; set; }
    public int UpdateScheduleDay { get; set; } = 15; // Ngày cập nhật hàng tháng
    public bool AutoUpdateEnabled { get; set; } = true;
    public string? LastUpdateStatus { get; set; }
    public DateTime? LastSuccessfulUpdate { get; set; }
    public DateTime LastModified { get; set; }
    
    // Web scraping configuration
    public string? PriceTableSelector { get; set; } // CSS selector cho bảng giá
    public string? PdfDownloadSelector { get; set; } // CSS selector cho link download PDF
    public string? ExcelDownloadSelector { get; set; } // CSS selector cho link download Excel
    
    // Navigation properties
    public virtual Province Province { get; set; } = null!;
}