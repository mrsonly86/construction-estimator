using System.ComponentModel.DataAnnotations;

namespace ConstructionEstimator.Core.Models
{
    /// <summary>
    /// Represents a construction material (Vật liệu xây dựng)
    /// </summary>
    public class Material
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(20)]
        public string Unit { get; set; } = string.Empty; // đơn vị: m3, m2, kg, tấn, v.v.

        public MaterialCategory Category { get; set; }

        [StringLength(100)]
        public string? Supplier { get; set; }

        // Current price per unit
        public decimal CurrentPrice { get; set; }

        // Origin/Manufacturer
        [StringLength(100)]
        public string? Origin { get; set; }

        // Quality/Grade
        [StringLength(50)]
        public string? Quality { get; set; }

        // Technical specifications
        [StringLength(1000)]
        public string? Specifications { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Price history
        public virtual ICollection<MaterialPrice> PriceHistory { get; set; } = new List<MaterialPrice>();
    }

    public enum MaterialCategory
    {
        Concrete = 1,           // Bê tông
        Steel = 2,              // Thép
        Cement = 3,             // Xi măng
        Sand = 4,               // Cát
        Stone = 5,              // Đá
        Brick = 6,              // Gạch
        Tile = 7,               // Ngói
        Wood = 8,               // Gỗ
        Paint = 9,              // Sơn
        Electrical = 10,        // Điện
        Plumbing = 11,          // Ống nước
        Roofing = 12,           // Lợp mái
        Flooring = 13,          // Sàn
        Windows = 14,           // Cửa sổ
        Doors = 15,             // Cửa
        Other = 99              // Khác
    }
}