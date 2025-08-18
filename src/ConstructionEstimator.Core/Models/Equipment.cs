using System.ComponentModel.DataAnnotations;

namespace ConstructionEstimator.Core.Models
{
    /// <summary>
    /// Represents construction equipment (Máy thi công)
    /// </summary>
    public class Equipment
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

        public EquipmentCategory Category { get; set; }

        [Required]
        [StringLength(20)]
        public string Unit { get; set; } = string.Empty; // ca, giờ, ngày, v.v.

        // Current rental/usage cost per unit
        public decimal CurrentCost { get; set; }

        [StringLength(100)]
        public string? Manufacturer { get; set; }

        [StringLength(50)]
        public string? Model { get; set; }

        // Technical specifications
        [StringLength(1000)]
        public string? Specifications { get; set; }

        // Fuel consumption per hour (if applicable)
        public decimal? FuelConsumption { get; set; }

        // Operator required
        public bool RequiresOperator { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Cost history
        public virtual ICollection<EquipmentCost> CostHistory { get; set; } = new List<EquipmentCost>();
    }

    public enum EquipmentCategory
    {
        Excavation = 1,         // Đào đất
        Lifting = 2,            // Nâng hạ
        Concrete = 3,           // Bê tông
        Transportation = 4,     // Vận chuyển
        Compaction = 5,         // Đầm nén
        Cutting = 6,            // Cắt
        Welding = 7,            // Hàn
        Pumping = 8,            // Bơm
        Drilling = 9,           // Khoan
        Paving = 10,            // Lát đường
        Other = 99              // Khác
    }

    /// <summary>
    /// Represents equipment cost at a specific time and location
    /// </summary>
    public class EquipmentCost
    {
        public int Id { get; set; }

        public int EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; } = null!;

        public decimal Cost { get; set; }

        [StringLength(100)]
        public string? Region { get; set; }

        [StringLength(100)]
        public string? Supplier { get; set; }

        public DateTime EffectiveDate { get; set; } = DateTime.Now;

        [StringLength(200)]
        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;
    }
}