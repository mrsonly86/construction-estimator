using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructionEstimator.Core.Entities;

public class Category : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty; // Material, Labor, Equipment, Standard
    
    public int? ParentCategoryId { get; set; }
    
    public int SortOrder { get; set; }
    
    [MaxLength(20)]
    public string? Color { get; set; }
    
    [MaxLength(100)]
    public string? Icon { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    [ForeignKey("ParentCategoryId")]
    public Category? ParentCategory { get; set; }
    
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
}