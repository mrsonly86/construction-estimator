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
    public DbSet<EstimateItem> EstimateItems { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Labor> Labor { get; set; }
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Standard> Standards { get; set; }
    public DbSet<StandardItem> StandardItems { get; set; }
    public DbSet<PriceList> PriceLists { get; set; }
    public DbSet<PriceListItem> PriceListItems { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConstructionEstimatorDbContext).Assembly);

        // Global query filters for soft delete
        modelBuilder.Entity<Project>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<EstimateItem>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Material>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Labor>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Equipment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Standard>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<StandardItem>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<PriceList>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<PriceListItem>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(e => !e.IsDeleted);

        // Seed initial data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed default categories
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Vật liệu xây dựng",
                Code = "MAT",
                Type = "Material",
                CreatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = 2,
                Name = "Nhân công",
                Code = "LAB",
                Type = "Labor",
                CreatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = 3,
                Name = "Máy móc thiết bị",
                Code = "EQP",
                Type = "Equipment",
                CreatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = 4,
                Name = "Định mức xây dựng",
                Code = "STD",
                Type = "Standard",
                CreatedAt = DateTime.UtcNow
            }
        );

        // Seed some basic materials
        modelBuilder.Entity<Material>().HasData(
            new Material
            {
                Id = 1,
                Name = "Xi măng PCB40",
                Code = "CEM001",
                Category = Core.Enums.MaterialCategory.Concrete,
                UnitOfMeasure = Core.Enums.UnitOfMeasure.Ton,
                UnitPrice = 2500000,
                Supplier = "Holcim Việt Nam",
                Brand = "Holcim",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Material
            {
                Id = 2,
                Name = "Thép CB300-V",
                Code = "STL001",
                Category = Core.Enums.MaterialCategory.Steel,
                UnitOfMeasure = Core.Enums.UnitOfMeasure.Ton,
                UnitPrice = 18000000,
                Supplier = "Hòa Phát",
                Brand = "Hòa Phát",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Material
            {
                Id = 3,
                Name = "Gạch ống 4 lỗ",
                Code = "BRC001",
                Category = Core.Enums.MaterialCategory.Brick,
                UnitOfMeasure = Core.Enums.UnitOfMeasure.Piece,
                UnitPrice = 1500,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        );

        // Seed some basic labor
        modelBuilder.Entity<Labor>().HasData(
            new Labor
            {
                Id = 1,
                Name = "Công nhân xây dựng",
                Code = "LAB001",
                Category = Core.Enums.LaborCategory.General,
                HourlyRate = 35000,
                DailyRate = 280000,
                MonthlyRate = 7000000,
                ProductivityFactor = 1.0m,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Labor
            {
                Id = 2,
                Name = "Thợ hàn",
                Code = "LAB002",
                Category = Core.Enums.LaborCategory.Skilled,
                HourlyRate = 50000,
                DailyRate = 400000,
                MonthlyRate = 10000000,
                ProductivityFactor = 1.0m,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Labor
            {
                Id = 3,
                Name = "Kỹ sư giám sát",
                Code = "LAB003",
                Category = Core.Enums.LaborCategory.Supervisor,
                HourlyRate = 100000,
                DailyRate = 800000,
                MonthlyRate = 20000000,
                ProductivityFactor = 1.0m,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        );
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}