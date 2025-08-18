namespace ConstructionEstimator.Core.Entities;

public class Material
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty; // Mã vật liệu theo tiêu chuẩn
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Unit { get; set; } = string.Empty; // Đơn vị tính (m3, m2, kg, tấn, etc.)
    public MaterialCategory Category { get; set; }
    public string? Specification { get; set; } // Thông số kỹ thuật
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual ICollection<MaterialPrice> MaterialPrices { get; set; } = new List<MaterialPrice>();
    public virtual ICollection<ProjectMaterial> ProjectMaterials { get; set; } = new List<ProjectMaterial>();
}

public enum MaterialCategory
{
    Concrete = 1,      // Bê tông
    Steel = 2,         // Thép
    Brick = 3,         // Gạch
    Sand = 4,          // Cát
    Stone = 5,         // Đá
    Cement = 6,        // Xi măng  
    Wood = 7,          // Gỗ
    Paint = 8,         // Sơn
    Tile = 9,          // Gạch ốp lát
    Electrical = 10,   // Điện
    Plumbing = 11,     // Nước
    Other = 99         // Khác
}