using Microsoft.EntityFrameworkCore;
using ConstructionEstimator.Core.Models;

namespace ConstructionEstimator.Data
{
    public interface IDataContext
    {
        DbSet<Project> Projects { get; set; }
        DbSet<Material> Materials { get; set; }
        DbSet<MaterialPrice> MaterialPrices { get; set; }
        DbSet<Labor> Labor { get; set; }
        DbSet<LaborCost> LaborCosts { get; set; }
        DbSet<Equipment> Equipment { get; set; }
        DbSet<EquipmentCost> EquipmentCosts { get; set; }
        DbSet<EstimateItem> EstimateItems { get; set; }
        DbSet<EstimateItemMaterial> EstimateItemMaterials { get; set; }
        DbSet<EstimateItemLabor> EstimateItemLabor { get; set; }
        DbSet<EstimateItemEquipment> EstimateItemEquipment { get; set; }
        DbSet<Standard> Standards { get; set; }
        DbSet<StandardMaterial> StandardMaterials { get; set; }
        DbSet<StandardLabor> StandardLabor { get; set; }
        DbSet<StandardEquipment> StandardEquipment { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public class SqliteDataContext : DbContext, IDataContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialPrice> MaterialPrices { get; set; }
        public DbSet<Labor> Labor { get; set; }
        public DbSet<LaborCost> LaborCosts { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentCost> EquipmentCosts { get; set; }
        public DbSet<EstimateItem> EstimateItems { get; set; }
        public DbSet<EstimateItemMaterial> EstimateItemMaterials { get; set; }
        public DbSet<EstimateItemLabor> EstimateItemLabor { get; set; }
        public DbSet<EstimateItemEquipment> EstimateItemEquipment { get; set; }
        public DbSet<Standard> Standards { get; set; }
        public DbSet<StandardMaterial> StandardMaterials { get; set; }
        public DbSet<StandardLabor> StandardLabor { get; set; }
        public DbSet<StandardEquipment> StandardEquipment { get; set; }

        public SqliteDataContext()
        {
        }

        public SqliteDataContext(DbContextOptions<SqliteDataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                    "ConstructionEstimator", "database.db");
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Project
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Client).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Contractor).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TotalCost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.VatRate).HasColumnType("decimal(5,2)");
                entity.Property(e => e.ProfitMargin).HasColumnType("decimal(5,2)");
                entity.Property(e => e.GeneralCostsRate).HasColumnType("decimal(5,2)");
            });

            // Configure Material
            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Unit).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CurrentPrice).HasColumnType("decimal(18,2)");
                entity.HasIndex(e => e.Code).IsUnique();
            });

            // Configure MaterialPrice
            modelBuilder.Entity<MaterialPrice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Material)
                    .WithMany(e => e.PriceHistory)
                    .HasForeignKey(e => e.MaterialId);
            });

            // Configure Labor
            modelBuilder.Entity<Labor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Unit).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CurrentCost).HasColumnType("decimal(18,2)");
                entity.HasIndex(e => e.Code).IsUnique();
            });

            // Configure LaborCost
            modelBuilder.Entity<LaborCost>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Cost).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Labor)
                    .WithMany(e => e.CostHistory)
                    .HasForeignKey(e => e.LaborId);
            });

            // Configure Equipment
            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Unit).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CurrentCost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.FuelConsumption).HasColumnType("decimal(10,2)");
                entity.HasIndex(e => e.Code).IsUnique();
            });

            // Configure EquipmentCost
            modelBuilder.Entity<EquipmentCost>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Cost).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Equipment)
                    .WithMany(e => e.CostHistory)
                    .HasForeignKey(e => e.EquipmentId);
            });

            // Configure EstimateItem
            modelBuilder.Entity<EstimateItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Unit).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Quantity).HasColumnType("decimal(18,4)");
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                
                entity.HasOne(e => e.Project)
                    .WithMany(e => e.EstimateItems)
                    .HasForeignKey(e => e.ProjectId);
                
                entity.HasOne(e => e.Parent)
                    .WithMany(e => e.Children)
                    .HasForeignKey(e => e.ParentId);
                
                entity.HasOne(e => e.Standard)
                    .WithMany()
                    .HasForeignKey(e => e.StandardId);
            });

            // Configure EstimateItemMaterial
            modelBuilder.Entity<EstimateItemMaterial>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.QuantityPerUnit).HasColumnType("decimal(18,4)");
                entity.Property(e => e.TotalQuantity).HasColumnType("decimal(18,4)");
                entity.Property(e => e.UnitCost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.WasteFactor).HasColumnType("decimal(5,2)");
                
                entity.HasOne(e => e.EstimateItem)
                    .WithMany(e => e.Materials)
                    .HasForeignKey(e => e.EstimateItemId);
                
                entity.HasOne(e => e.Material)
                    .WithMany()
                    .HasForeignKey(e => e.MaterialId);
            });

            // Configure EstimateItemLabor
            modelBuilder.Entity<EstimateItemLabor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.QuantityPerUnit).HasColumnType("decimal(18,4)");
                entity.Property(e => e.TotalQuantity).HasColumnType("decimal(18,4)");
                entity.Property(e => e.UnitCost).HasColumnType("decimal(18,2)");
                
                entity.HasOne(e => e.EstimateItem)
                    .WithMany(e => e.Labor)
                    .HasForeignKey(e => e.EstimateItemId);
                
                entity.HasOne(e => e.Labor)
                    .WithMany()
                    .HasForeignKey(e => e.LaborId);
            });

            // Configure EstimateItemEquipment
            modelBuilder.Entity<EstimateItemEquipment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.QuantityPerUnit).HasColumnType("decimal(18,4)");
                entity.Property(e => e.TotalQuantity).HasColumnType("decimal(18,4)");
                entity.Property(e => e.UnitCost).HasColumnType("decimal(18,2)");
                
                entity.HasOne(e => e.EstimateItem)
                    .WithMany(e => e.Equipment)
                    .HasForeignKey(e => e.EstimateItemId);
                
                entity.HasOne(e => e.Equipment)
                    .WithMany()
                    .HasForeignKey(e => e.EquipmentId);
            });

            // Configure Standard
            modelBuilder.Entity<Standard>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Unit).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Version).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Code).IsUnique();
            });

            // Configure StandardMaterial
            modelBuilder.Entity<StandardMaterial>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.QuantityPerUnit).HasColumnType("decimal(18,4)");
                entity.Property(e => e.WasteFactor).HasColumnType("decimal(5,2)");
                
                entity.HasOne(e => e.Standard)
                    .WithMany(e => e.Materials)
                    .HasForeignKey(e => e.StandardId);
                
                entity.HasOne(e => e.Material)
                    .WithMany()
                    .HasForeignKey(e => e.MaterialId);
            });

            // Configure StandardLabor
            modelBuilder.Entity<StandardLabor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.QuantityPerUnit).HasColumnType("decimal(18,4)");
                
                entity.HasOne(e => e.Standard)
                    .WithMany(e => e.Labor)
                    .HasForeignKey(e => e.StandardId);
                
                entity.HasOne(e => e.Labor)
                    .WithMany()
                    .HasForeignKey(e => e.LaborId);
            });

            // Configure StandardEquipment
            modelBuilder.Entity<StandardEquipment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.QuantityPerUnit).HasColumnType("decimal(18,4)");
                
                entity.HasOne(e => e.Standard)
                    .WithMany(e => e.Equipment)
                    .HasForeignKey(e => e.StandardId);
                
                entity.HasOne(e => e.Equipment)
                    .WithMany()
                    .HasForeignKey(e => e.EquipmentId);
            });
        }
    }
}