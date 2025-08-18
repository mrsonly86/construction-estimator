using System.ComponentModel.DataAnnotations;

namespace ConstructionEstimator.Core.Models
{
    /// <summary>
    /// Represents an estimate item/work item (Hạng mục dự toán)
    /// </summary>
    public class EstimateItem
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; } = null!;

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

        // Quantity of work
        public decimal Quantity { get; set; }

        // Unit price
        public decimal UnitPrice { get; set; }

        // Total amount for this item
        public decimal TotalAmount => Quantity * UnitPrice;

        // Category/Group
        [StringLength(100)]
        public string? Category { get; set; }

        // Parent item for hierarchical structure
        public int? ParentId { get; set; }
        public virtual EstimateItem? Parent { get; set; }

        // Child items
        public virtual ICollection<EstimateItem> Children { get; set; } = new List<EstimateItem>();

        // Order for sorting
        public int SortOrder { get; set; }

        // Associated standard/norm
        public int? StandardId { get; set; }
        public virtual Standard? Standard { get; set; }

        // Materials required for this item
        public virtual ICollection<EstimateItemMaterial> Materials { get; set; } = new List<EstimateItemMaterial>();

        // Labor required for this item
        public virtual ICollection<EstimateItemLabor> Labor { get; set; } = new List<EstimateItemLabor>();

        // Equipment required for this item
        public virtual ICollection<EstimateItemEquipment> Equipment { get; set; } = new List<EstimateItemEquipment>();

        // Notes
        [StringLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// Junction table for EstimateItem and Material
    /// </summary>
    public class EstimateItemMaterial
    {
        public int Id { get; set; }

        public int EstimateItemId { get; set; }
        public virtual EstimateItem EstimateItem { get; set; } = null!;

        public int MaterialId { get; set; }
        public virtual Material Material { get; set; } = null!;

        // Quantity of material needed per unit of work
        public decimal QuantityPerUnit { get; set; }

        // Total quantity needed
        public decimal TotalQuantity { get; set; }

        // Cost per unit of material
        public decimal UnitCost { get; set; }

        // Total cost for this material
        public decimal TotalCost => TotalQuantity * UnitCost;

        // Waste factor (percentage)
        public decimal WasteFactor { get; set; } = 5; // Default 5% waste

        [StringLength(200)]
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Junction table for EstimateItem and Labor
    /// </summary>
    public class EstimateItemLabor
    {
        public int Id { get; set; }

        public int EstimateItemId { get; set; }
        public virtual EstimateItem EstimateItem { get; set; } = null!;

        public int LaborId { get; set; }
        public virtual Labor Labor { get; set; } = null!;

        // Quantity of labor needed per unit of work
        public decimal QuantityPerUnit { get; set; }

        // Total quantity needed
        public decimal TotalQuantity { get; set; }

        // Cost per unit of labor
        public decimal UnitCost { get; set; }

        // Total cost for this labor
        public decimal TotalCost => TotalQuantity * UnitCost;

        [StringLength(200)]
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Junction table for EstimateItem and Equipment
    /// </summary>
    public class EstimateItemEquipment
    {
        public int Id { get; set; }

        public int EstimateItemId { get; set; }
        public virtual EstimateItem EstimateItem { get; set; } = null!;

        public int EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; } = null!;

        // Quantity of equipment needed per unit of work
        public decimal QuantityPerUnit { get; set; }

        // Total quantity needed
        public decimal TotalQuantity { get; set; }

        // Cost per unit of equipment usage
        public decimal UnitCost { get; set; }

        // Total cost for this equipment
        public decimal TotalCost => TotalQuantity * UnitCost;

        [StringLength(200)]
        public string? Notes { get; set; }
    }
}