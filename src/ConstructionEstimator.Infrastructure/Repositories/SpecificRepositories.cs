using Microsoft.EntityFrameworkCore;
using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Interfaces;
using ConstructionEstimator.Core.Enums;
using ConstructionEstimator.Infrastructure.Data;

namespace ConstructionEstimator.Infrastructure.Repositories;

public class ProjectRepository : Repository<Project>, IProjectRepository
{
    public ProjectRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatus status)
    {
        return await _dbSet
            .Where(p => p.Status == status)
            .ToListAsync();
    }

    public async Task<Project?> GetProjectWithSectionsAsync(int projectId)
    {
        return await _dbSet
            .Include(p => p.Sections)
                .ThenInclude(s => s.Items)
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }
}

public class MaterialRepository : Repository<Material>, IMaterialRepository
{
    public MaterialRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Material>> GetMaterialsByCategoryAsync(string category)
    {
        return await _dbSet
            .Where(m => m.Category == category)
            .ToListAsync();
    }

    public async Task<IEnumerable<Material>> SearchMaterialsAsync(string searchTerm)
    {
        return await _dbSet
            .Where(m => m.Name.Contains(searchTerm) || m.Code.Contains(searchTerm))
            .ToListAsync();
    }
}

public class LaborRepository : Repository<Labor>, ILaborRepository
{
    public LaborRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Labor>> GetLaborsByCategoryAsync(string category)
    {
        return await _dbSet
            .Where(l => l.Category == category)
            .ToListAsync();
    }

    public async Task<IEnumerable<Labor>> GetLaborsBySkillLevelAsync(LaborSkillLevel skillLevel)
    {
        return await _dbSet
            .Where(l => l.SkillLevel == skillLevel)
            .ToListAsync();
    }
}

// New repository implementations for Material Price Auto-Update System
public class ProvinceRepository : Repository<Province>, IProvinceRepository
{
    public ProvinceRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<Province?> GetProvinceByCodeAsync(string code)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.Code == code);
    }

    public async Task<IEnumerable<Province>> GetProvincesByRegionAsync(string region)
    {
        return await _dbSet
            .Where(p => p.Region == region)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Province>> GetActiveProvincesAsync()
    {
        return await _dbSet
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
}

public class MaterialPriceRepository : Repository<MaterialPrice>, IMaterialPriceRepository
{
    public MaterialPriceRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MaterialPrice>> GetMaterialPricesByProvinceAsync(int provinceId)
    {
        return await _dbSet
            .Include(mp => mp.Material)
            .Include(mp => mp.Province)
            .Where(mp => mp.ProvinceId == provinceId)
            .OrderBy(mp => mp.Material.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<MaterialPrice>> GetMaterialPricesForMaterialAsync(int materialId, int? provinceId = null)
    {
        var query = _dbSet
            .Include(mp => mp.Material)
            .Include(mp => mp.Province)
            .Where(mp => mp.MaterialId == materialId);

        if (provinceId.HasValue)
        {
            query = query.Where(mp => mp.ProvinceId == provinceId.Value);
        }

        return await query
            .OrderBy(mp => mp.Province.Name)
            .ThenByDescending(mp => mp.EffectiveDate)
            .ToListAsync();
    }

    public async Task<MaterialPrice?> GetCurrentPriceAsync(int materialId, int provinceId)
    {
        return await _dbSet
            .Include(mp => mp.Material)
            .Include(mp => mp.Province)
            .Where(mp => mp.MaterialId == materialId && mp.ProvinceId == provinceId)
            .Where(mp => mp.EffectiveDate <= DateTime.Now && (mp.EndDate == null || mp.EndDate > DateTime.Now))
            .OrderByDescending(mp => mp.EffectiveDate)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<MaterialPrice>> GetPricesEffectiveOnDateAsync(DateTime date, int? materialId = null, int? provinceId = null)
    {
        var query = _dbSet
            .Include(mp => mp.Material)
            .Include(mp => mp.Province)
            .Where(mp => mp.EffectiveDate <= date && (mp.EndDate == null || mp.EndDate > date));

        if (materialId.HasValue)
        {
            query = query.Where(mp => mp.MaterialId == materialId.Value);
        }

        if (provinceId.HasValue)
        {
            query = query.Where(mp => mp.ProvinceId == provinceId.Value);
        }

        return await query
            .OrderBy(mp => mp.Material.Name)
            .ThenBy(mp => mp.Province.Name)
            .ToListAsync();
    }
}

public class PriceHistoryRepository : Repository<PriceHistory>, IPriceHistoryRepository
{
    public PriceHistoryRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PriceHistory>> GetPriceHistoryAsync(int materialId, int? provinceId = null)
    {
        var query = _dbSet
            .Include(ph => ph.MaterialPrice)
                .ThenInclude(mp => mp.Material)
            .Include(ph => ph.MaterialPrice)
                .ThenInclude(mp => mp.Province)
            .Where(ph => ph.MaterialPrice.MaterialId == materialId);

        if (provinceId.HasValue)
        {
            query = query.Where(ph => ph.MaterialPrice.ProvinceId == provinceId.Value);
        }

        return await query
            .OrderByDescending(ph => ph.ChangeDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<PriceHistory>> GetRecentPriceChangesAsync(DateTime fromDate, int? provinceId = null)
    {
        var query = _dbSet
            .Include(ph => ph.MaterialPrice)
                .ThenInclude(mp => mp.Material)
            .Include(ph => ph.MaterialPrice)
                .ThenInclude(mp => mp.Province)
            .Where(ph => ph.ChangeDate >= fromDate);

        if (provinceId.HasValue)
        {
            query = query.Where(ph => ph.MaterialPrice.ProvinceId == provinceId.Value);
        }

        return await query
            .OrderByDescending(ph => ph.ChangeDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<PriceHistory>> GetSignificantPriceChangesAsync(decimal thresholdPercentage, DateTime fromDate)
    {
        return await _dbSet
            .Include(ph => ph.MaterialPrice)
                .ThenInclude(mp => mp.Material)
            .Include(ph => ph.MaterialPrice)
                .ThenInclude(mp => mp.Province)
            .Where(ph => ph.ChangeDate >= fromDate)
            .Where(ph => Math.Abs(ph.ChangePercentage) >= thresholdPercentage)
            .OrderByDescending(ph => Math.Abs(ph.ChangePercentage))
            .ToListAsync();
    }

    public async Task<decimal> GetAveragePriceChangeAsync(int materialId, int days)
    {
        var fromDate = DateTime.Now.AddDays(-days);
        
        var changes = await _dbSet
            .Where(ph => ph.MaterialPrice.MaterialId == materialId)
            .Where(ph => ph.ChangeDate >= fromDate)
            .Select(ph => ph.ChangePercentage)
            .ToListAsync();

        return changes.Any() ? changes.Average() : 0;
    }
}

public class PriceAlertRepository : Repository<PriceAlert>, IPriceAlertRepository
{
    public PriceAlertRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PriceAlert>> GetActiveAlertsAsync()
    {
        return await _dbSet
            .Include(pa => pa.Material)
            .Include(pa => pa.Province)
            .Where(pa => pa.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<PriceAlert>> GetAlertsForMaterialAsync(int materialId)
    {
        return await _dbSet
            .Include(pa => pa.Material)
            .Include(pa => pa.Province)
            .Where(pa => pa.MaterialId == materialId && pa.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<PriceAlert>> GetAlertsForProvinceAsync(int provinceId)
    {
        return await _dbSet
            .Include(pa => pa.Material)
            .Include(pa => pa.Province)
            .Where(pa => (pa.ProvinceId == provinceId || pa.ProvinceId == null) && pa.IsActive)
            .ToListAsync();
    }
}

public class DataSourceRepository : Repository<DataSource>, IDataSourceRepository
{
    public DataSourceRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DataSource>> GetDataSourcesByProvinceAsync(int provinceId)
    {
        return await _dbSet
            .Include(ds => ds.Province)
            .Where(ds => ds.ProvinceId == provinceId)
            .OrderBy(ds => ds.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<DataSource>> GetActiveDataSourcesAsync()
    {
        return await _dbSet
            .Include(ds => ds.Province)
            .Where(ds => ds.IsActive)
            .OrderBy(ds => ds.Province.Name)
            .ThenBy(ds => ds.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<DataSource>> GetDataSourcesDueForUpdateAsync()
    {
        var now = DateTime.Now;
        return await _dbSet
            .Include(ds => ds.Province)
            .Where(ds => ds.IsActive && ds.NextScanDate <= now)
            .OrderBy(ds => ds.NextScanDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<DataSource>> GetDataSourcesByTypeAsync(string sourceType)
    {
        return await _dbSet
            .Include(ds => ds.Province)
            .Where(ds => ds.SourceType == sourceType && ds.IsActive)
            .OrderBy(ds => ds.Province.Name)
            .ThenBy(ds => ds.Name)
            .ToListAsync();
    }
}