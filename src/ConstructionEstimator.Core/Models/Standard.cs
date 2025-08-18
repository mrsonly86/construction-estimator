using System.ComponentModel.DataAnnotations;

namespace ConstructionEstimator.Core.Models
{
    /// <summary>
    /// Represents construction standards/norms (Định mức xây dựng)
    /// </summary>
    public class Standard
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [StringLength(20)]
        public string Unit { get; set; } = string.Empty;

        // Category/Type of work
        public StandardCategory Category { get; set; }

        // Formula for calculation (if applicable)
        [StringLength(500)]
        public string? Formula { get; set; }

        // Materials required according to standard
        public virtual ICollection<StandardMaterial> Materials { get; set; } = new List<StandardMaterial>();

        // Labor required according to standard
        public virtual ICollection<StandardLabor> Labor { get; set; } = new List<StandardLabor>();

        // Equipment required according to standard
        public virtual ICollection<StandardEquipment> Equipment { get; set; } = new List<StandardEquipment>();

        // Version/Revision
        [StringLength(20)]
        public string Version { get; set; } = "1.0";

        // Authority that issued the standard
        [StringLength(100)]
        public string? IssuedBy { get; set; }

        public DateTime IssuedDate { get; set; } = DateTime.Now;
        public DateTime EffectiveDate { get; set; } = DateTime.Now;
        public DateTime? ExpiryDate { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(1000)]
        public string? Notes { get; set; }
    }

    public enum StandardCategory
    {
        Earthwork = 1,          // Công tác đất
        Foundation = 2,         // Móng
        Concrete = 3,           // Bê tông
        Masonry = 4,            // Xây
        Roofing = 5,            // Lợp mái
        Finishing = 6,          // Hoàn thiện
        Electrical = 7,         // Điện
        Plumbing = 8,           // Nước
        HVAC = 9,               // Điều hòa
        Landscaping = 10,       // Cảnh quan
        Other = 99              // Khác
    }

    /// <summary>
    /// Materials defined in a standard
    /// </summary>
    public class StandardMaterial
    {
        public int Id { get; set; }

        public int StandardId { get; set; }
        public virtual Standard Standard { get; set; } = null!;

        public int MaterialId { get; set; }
        public virtual Material Material { get; set; } = null!;

        // Quantity per unit of work according to standard
        public decimal QuantityPerUnit { get; set; }

        // Waste factor
        public decimal WasteFactor { get; set; } = 5;

        [StringLength(200)]
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Labor defined in a standard
    /// </summary>
    public class StandardLabor
    {
        public int Id { get; set; }

        public int StandardId { get; set; }
        public virtual Standard Standard { get; set; } = null!;

        public int LaborId { get; set; }
        public virtual Labor Labor { get; set; } = null!;

        // Quantity per unit of work according to standard
        public decimal QuantityPerUnit { get; set; }

        [StringLength(200)]
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Equipment defined in a standard
    /// </summary>
    public class StandardEquipment
    {
        public int Id { get; set; }

        public int StandardId { get; set; }
        public virtual Standard Standard { get; set; } = null!;

        public int EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; } = null!;

        // Quantity per unit of work according to standard
        public decimal QuantityPerUnit { get; set; }

        [StringLength(200)]
        public string? Notes { get; set; }
    }
}