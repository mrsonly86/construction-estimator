using System.ComponentModel.DataAnnotations;

namespace ConstructionEstimator.Core.Models
{
    /// <summary>
    /// Represents a construction project (Dự án xây dựng)
    /// </summary>
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Client { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Contractor { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string? CreatedBy { get; set; }

        [StringLength(50)]
        public string? LastModifiedBy { get; set; }

        public ProjectStatus Status { get; set; } = ProjectStatus.Draft;

        // Total estimated cost
        public decimal TotalCost { get; set; }

        // VAT percentage
        public decimal VatRate { get; set; } = 10; // Default 10% VAT

        // Profit margin percentage
        public decimal ProfitMargin { get; set; } = 15; // Default 15% profit

        // General costs percentage
        public decimal GeneralCostsRate { get; set; } = 8; // Default 8% general costs

        // Collection of estimate items
        public virtual ICollection<EstimateItem> EstimateItems { get; set; } = new List<EstimateItem>();

        // Project settings as JSON
        [StringLength(2000)]
        public string? Settings { get; set; }
    }

    public enum ProjectStatus
    {
        Draft = 0,
        InProgress = 1,
        UnderReview = 2,
        Approved = 3,
        Completed = 4,
        Cancelled = 5
    }
}