using Microsoft.EntityFrameworkCore;
using ConstructionEstimator.Core.Entities;

namespace ConstructionEstimator.Infrastructure.Data;

public class ConstructionEstimatorDbContext : DbContext
{
    public ConstructionEstimatorDbContext(DbContextOptions<ConstructionEstimatorDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<EstimateSection> EstimateSections { get; set; }
    public DbSet<EstimateItem> EstimateItems { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Labor> Labors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Project configuration
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Location).HasMaxLength(300);
            entity.Property(e => e.Client).HasMaxLength(200);
            entity.Property(e => e.TotalEstimatedCost).HasColumnType("decimal(18,2)");
        });

        // EstimateSection configuration
        modelBuilder.Entity<EstimateSection>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.HasOne(e => e.Project)
                  .WithMany(p => p.Sections)
                  .HasForeignKey(e => e.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // EstimateItem configuration
        modelBuilder.Entity<EstimateItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Unit).HasMaxLength(20);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18,3)");
            entity.Property(e => e.MaterialCost).HasColumnType("decimal(18,2)");
            entity.Property(e => e.LaborCost).HasColumnType("decimal(18,2)");
            entity.Property(e => e.EquipmentCost).HasColumnType("decimal(18,2)");
            entity.HasOne(e => e.Section)
                  .WithMany(s => s.Items)
                  .HasForeignKey(e => e.SectionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Material configuration
        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Unit).HasMaxLength(20);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Supplier).HasMaxLength(200);
            entity.Property(e => e.Category).HasMaxLength(100);
        });

        // Labor configuration
        modelBuilder.Entity<Labor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.HourlyRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.DailyRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Category).HasMaxLength(100);
        });
    }
}