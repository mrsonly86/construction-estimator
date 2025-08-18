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
    
    // New DbSets for Material Price Auto-Update System
    public DbSet<Province> Provinces { get; set; }
    public DbSet<MaterialPrice> MaterialPrices { get; set; }
    public DbSet<PriceHistory> PriceHistories { get; set; }
    public DbSet<PriceAlert> PriceAlerts { get; set; }
    public DbSet<DataSource> DataSources { get; set; }

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
            entity.Property(e => e.Keywords).HasMaxLength(500);
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

        // Province configuration
        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Region).HasMaxLength(50);
            entity.HasIndex(e => e.Code).IsUnique();
        });

        // MaterialPrice configuration
        modelBuilder.Entity<MaterialPrice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Supplier).HasMaxLength(200);
            entity.Property(e => e.DataSourceUrl).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            
            entity.HasOne(e => e.Material)
                  .WithMany(m => m.ProvincesPrices)
                  .HasForeignKey(e => e.MaterialId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Province)
                  .WithMany(p => p.MaterialPrices)
                  .HasForeignKey(e => e.ProvinceId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.MaterialId, e.ProvinceId, e.EffectiveDate })
                  .HasDatabaseName("IX_MaterialPrice_Material_Province_Date");
        });

        // PriceHistory configuration
        modelBuilder.Entity<PriceHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OldPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.NewPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ChangeAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ChangePercentage).HasColumnType("decimal(5,2)");
            entity.Property(e => e.ChangeType).HasMaxLength(20);
            entity.Property(e => e.Source).HasMaxLength(200);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            
            entity.HasOne(e => e.MaterialPrice)
                  .WithMany()
                  .HasForeignKey(e => e.MaterialPriceId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.ChangeDate)
                  .HasDatabaseName("IX_PriceHistory_ChangeDate");
        });

        // PriceAlert configuration
        modelBuilder.Entity<PriceAlert>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AlertType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ThresholdPercentage).HasColumnType("decimal(5,2)");
            entity.Property(e => e.ThresholdAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.NotificationEmail).HasMaxLength(200);
            
            entity.HasOne(e => e.Material)
                  .WithMany(m => m.PriceAlerts)
                  .HasForeignKey(e => e.MaterialId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Province)
                  .WithMany()
                  .HasForeignKey(e => e.ProvinceId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // DataSource configuration
        modelBuilder.Entity<DataSource>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.SourceType).IsRequired().HasMaxLength(20);
            entity.Property(e => e.SourceUrl).HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ScanConfiguration).HasColumnType("text");
            
            entity.HasOne(e => e.Province)
                  .WithMany(p => p.DataSources)
                  .HasForeignKey(e => e.ProvinceId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}