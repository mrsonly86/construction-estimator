using Microsoft.EntityFrameworkCore;
using ConstructionEstimator.Core.Entities;

namespace ConstructionEstimator.Data.Context;

public class ConstructionEstimatorDbContext : DbContext
{
    public ConstructionEstimatorDbContext(DbContextOptions<ConstructionEstimatorDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<MaterialPrice> MaterialPrices { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<ProvinceConfig> ProvinceConfigs { get; set; }
    public DbSet<ProjectMaterial> ProjectMaterials { get; set; }
    public DbSet<CostEstimate> CostEstimates { get; set; }
    public DbSet<PriceHistory> PriceHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Province
        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Code);
            entity.Property(e => e.Code).HasMaxLength(2);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.Region).HasMaxLength(50);
        });

        // Configure ProvinceConfig
        modelBuilder.Entity<ProvinceConfig>(entity =>
        {
            entity.Property(e => e.ProvinceCode).HasMaxLength(2).IsRequired();
            entity.Property(e => e.DepartmentName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.WebsiteUrl).HasMaxLength(500);
            entity.Property(e => e.PriceListUrl).HasMaxLength(500);
            entity.Property(e => e.BackupUrl).HasMaxLength(500);
            
            entity.HasOne(e => e.Province)
                  .WithOne(p => p.ProvinceConfig)
                  .HasForeignKey<ProvinceConfig>(e => e.ProvinceCode);
        });

        // Configure Material
        modelBuilder.Entity<Material>(entity =>
        {
            entity.Property(e => e.Code).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Unit).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Specification).HasMaxLength(1000);
            
            entity.HasIndex(e => e.Code).IsUnique();
        });

        // Configure MaterialPrice
        modelBuilder.Entity<MaterialPrice>(entity =>
        {
            entity.Property(e => e.ProvinceCode).HasMaxLength(2).IsRequired();
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.Source).HasMaxLength(100).IsRequired();
            entity.Property(e => e.SourceUrl).HasMaxLength(500);
            
            entity.HasOne(e => e.Material)
                  .WithMany(m => m.MaterialPrices)
                  .HasForeignKey(e => e.MaterialId);
                  
            entity.HasOne(e => e.Province)
                  .WithMany(p => p.MaterialPrices)
                  .HasForeignKey(e => e.ProvinceCode);
        });

        // Configure Project
        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Location).HasMaxLength(300).IsRequired();
            entity.Property(e => e.ProvinceCode).HasMaxLength(2).IsRequired();
            entity.Property(e => e.EstimatedBudget).HasPrecision(18, 2);
            entity.Property(e => e.ActualCost).HasPrecision(18, 2);
            entity.Property(e => e.Description).HasMaxLength(1000);
        });

        // Configure ProjectMaterial
        modelBuilder.Entity<ProjectMaterial>(entity =>
        {
            entity.Property(e => e.Quantity).HasPrecision(18, 4);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.TotalCost).HasPrecision(18, 2);
            
            entity.HasOne(e => e.Project)
                  .WithMany(p => p.ProjectMaterials)
                  .HasForeignKey(e => e.ProjectId);
                  
            entity.HasOne(e => e.Material)
                  .WithMany(m => m.ProjectMaterials)
                  .HasForeignKey(e => e.MaterialId);
        });

        // Configure CostEstimate
        modelBuilder.Entity<CostEstimate>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.MaterialCost).HasPrecision(18, 2);
            entity.Property(e => e.LaborCost).HasPrecision(18, 2);
            entity.Property(e => e.EquipmentCost).HasPrecision(18, 2);
            entity.Property(e => e.OverheadCost).HasPrecision(18, 2);
            entity.Property(e => e.ProfitMargin).HasPrecision(18, 2);
            entity.Property(e => e.TotalCost).HasPrecision(18, 2);
            
            entity.HasOne(e => e.Project)
                  .WithMany(p => p.CostEstimates)
                  .HasForeignKey(e => e.ProjectId);
        });

        // Configure PriceHistory
        modelBuilder.Entity<PriceHistory>(entity =>
        {
            entity.Property(e => e.ProvinceCode).HasMaxLength(2).IsRequired();
            entity.Property(e => e.OldPrice).HasPrecision(18, 2);
            entity.Property(e => e.NewPrice).HasPrecision(18, 2);
            entity.Property(e => e.ChangePercentage).HasPrecision(18, 4);
            entity.Property(e => e.Source).HasMaxLength(100).IsRequired();
            
            entity.HasOne(e => e.Material)
                  .WithMany()
                  .HasForeignKey(e => e.MaterialId);
                  
            entity.HasOne(e => e.Province)
                  .WithMany()
                  .HasForeignKey(e => e.ProvinceCode);
        });

        // Seed initial data
        SeedInitialData(modelBuilder);
    }

    private static void SeedInitialData(ModelBuilder modelBuilder)
    {
        // Seed Vietnamese provinces
        modelBuilder.Entity<Province>().HasData(
            new Province { Code = "01", Name = "Hà Nội", FullName = "Thành phố Hà Nội", Region = "Miền Bắc" },
            new Province { Code = "79", Name = "TP. Hồ Chí Minh", FullName = "Thành phố Hồ Chí Minh", Region = "Miền Nam" },
            new Province { Code = "48", Name = "Đà Nẵng", FullName = "Thành phố Đà Nẵng", Region = "Miền Trung" },
            new Province { Code = "31", Name = "Hải Phòng", FullName = "Thành phố Hải Phòng", Region = "Miền Bắc" },
            new Province { Code = "92", Name = "Cần Thơ", FullName = "Thành phố Cần Thơ", Region = "Miền Nam" }
            // Add more provinces as needed
        );

        // Seed common construction materials
        modelBuilder.Entity<Material>().HasData(
            new Material { Id = 1, Code = "BT_B25", Name = "Bê tông thương phẩm B25", Unit = "m3", Category = MaterialCategory.Concrete },
            new Material { Id = 2, Code = "THEP_CB300V", Name = "Thép thanh vằn CB300-V", Unit = "kg", Category = MaterialCategory.Steel },
            new Material { Id = 3, Code = "GACH_XD", Name = "Gạch xây dựng", Unit = "viên", Category = MaterialCategory.Brick },
            new Material { Id = 4, Code = "CAT_XD", Name = "Cát xây dựng", Unit = "m3", Category = MaterialCategory.Sand },
            new Material { Id = 5, Code = "XI_MANG_PCB40", Name = "Xi măng PCB40", Unit = "kg", Category = MaterialCategory.Cement }
        );
    }
}