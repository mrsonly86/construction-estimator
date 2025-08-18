using System.ComponentModel.DataAnnotations;

namespace ConstructionEstimator.Core.Models
{
    /// <summary>
    /// Represents a material price at a specific time and location
    /// </summary>
    public class MaterialPrice
    {
        public int Id { get; set; }

        public int MaterialId { get; set; }
        public virtual Material Material { get; set; } = null!;

        public decimal Price { get; set; }

        [StringLength(100)]
        public string? Region { get; set; } // Khu vực: Hà Nội, TP.HCM, v.v.

        [StringLength(100)]
        public string? Supplier { get; set; }

        public DateTime EffectiveDate { get; set; } = DateTime.Now;

        [StringLength(200)]
        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// Represents labor cost (Chi phí nhân công)
    /// </summary>
    public class Labor
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

        public LaborType Type { get; set; }

        [Required]
        [StringLength(20)]
        public string Unit { get; set; } = string.Empty; // công/ngày, giờ, m2, m3, v.v.

        // Current cost per unit
        public decimal CurrentCost { get; set; }

        // Skill level required
        public SkillLevel SkillLevel { get; set; }

        [StringLength(100)]
        public string? Region { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Cost history
        public virtual ICollection<LaborCost> CostHistory { get; set; } = new List<LaborCost>();
    }

    public enum LaborType
    {
        Unskilled = 1,      // Phổ thông
        Skilled = 2,        // Có tay nghề
        Specialized = 3,    // Chuyên môn
        Management = 4      // Quản lý
    }

    public enum SkillLevel
    {
        Level1 = 1,         // Bậc 1
        Level2 = 2,         // Bậc 2
        Level3 = 3,         // Bậc 3
        Level4 = 4,         // Bậc 4
        Level5 = 5,         // Bậc 5
        Level6 = 6,         // Bậc 6
        Level7 = 7          // Bậc 7
    }

    /// <summary>
    /// Represents labor cost at a specific time and location
    /// </summary>
    public class LaborCost
    {
        public int Id { get; set; }

        public int LaborId { get; set; }
        public virtual Labor Labor { get; set; } = null!;

        public decimal Cost { get; set; }

        [StringLength(100)]
        public string? Region { get; set; }

        public DateTime EffectiveDate { get; set; } = DateTime.Now;

        [StringLength(200)]
        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;
    }
}