using Microsoft.EntityFrameworkCore;
using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Enums;
using ConstructionEstimator.Core.Interfaces;
using ConstructionEstimator.Data.Context;

namespace ConstructionEstimator.Data.Repositories;

public class ProjectRepository : Repository<Project>, IProjectRepository
{
    public ProjectRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Project>> GetProjectsByClientAsync(string clientName)
    {
        return await _dbSet
            .Where(p => p.ClientName.Contains(clientName))
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatus status)
    {
        return await _dbSet
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Project?> GetProjectWithItemsAsync(int projectId)
    {
        return await _dbSet
            .Include(p => p.EstimateItems)
                .ThenInclude(ei => ei.Material)
            .Include(p => p.EstimateItems)
                .ThenInclude(ei => ei.Labor)
            .Include(p => p.EstimateItems)
                .ThenInclude(ei => ei.Equipment)
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }
}

public class MaterialRepository : Repository<Material>, IMaterialRepository
{
    public MaterialRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Material>> GetMaterialsByCategoryAsync(MaterialCategory category)
    {
        return await _dbSet
            .Where(m => m.Category == category && m.IsActive)
            .OrderBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Material>> GetActiveMaterialsAsync()
    {
        return await _dbSet
            .Where(m => m.IsActive)
            .OrderBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<Material?> GetMaterialByCodeAsync(string code)
    {
        return await _dbSet
            .FirstOrDefaultAsync(m => m.Code == code);
    }
}

public class LaborRepository : Repository<Labor>, ILaborRepository
{
    public LaborRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Labor>> GetLaborByCategoryAsync(LaborCategory category)
    {
        return await _dbSet
            .Where(l => l.Category == category && l.IsActive)
            .OrderBy(l => l.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Labor>> GetActiveLaborAsync()
    {
        return await _dbSet
            .Where(l => l.IsActive)
            .OrderBy(l => l.Name)
            .ToListAsync();
    }

    public async Task<Labor?> GetLaborByCodeAsync(string code)
    {
        return await _dbSet
            .FirstOrDefaultAsync(l => l.Code == code);
    }
}

public class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
{
    public EquipmentRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Equipment>> GetActiveEquipmentAsync()
    {
        return await _dbSet
            .Where(e => e.IsActive)
            .OrderBy(e => e.Name)
            .ToListAsync();
    }

    public async Task<Equipment?> GetEquipmentByCodeAsync(string code)
    {
        return await _dbSet
            .FirstOrDefaultAsync(e => e.Code == code);
    }
}

public class StandardRepository : Repository<Standard>, IStandardRepository
{
    public StandardRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Standard>> GetStandardsByCategoryAsync(string category)
    {
        return await _dbSet
            .Where(s => s.Category == category && s.IsActive)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Standard>> GetActiveStandardsAsync()
    {
        return await _dbSet
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<Standard?> GetStandardByCodeAsync(string code)
    {
        return await _dbSet
            .FirstOrDefaultAsync(s => s.Code == code);
    }

    public async Task<Standard?> GetStandardWithItemsAsync(int standardId)
    {
        return await _dbSet
            .Include(s => s.StandardItems)
                .ThenInclude(si => si.Material)
            .Include(s => s.StandardItems)
                .ThenInclude(si => si.Labor)
            .Include(s => s.StandardItems)
                .ThenInclude(si => si.Equipment)
            .FirstOrDefaultAsync(s => s.Id == standardId);
    }
}

public class PriceListRepository : Repository<PriceList>, IPriceListRepository
{
    public PriceListRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PriceList>> GetActivePriceListsAsync()
    {
        return await _dbSet
            .Where(pl => pl.IsActive)
            .OrderByDescending(pl => pl.EffectiveDate)
            .ToListAsync();
    }

    public async Task<PriceList?> GetDefaultPriceListAsync(PriceListType type)
    {
        return await _dbSet
            .FirstOrDefaultAsync(pl => pl.Type == type && pl.IsDefault && pl.IsActive);
    }

    public async Task<PriceList?> GetPriceListWithItemsAsync(int priceListId)
    {
        return await _dbSet
            .Include(pl => pl.PriceListItems)
                .ThenInclude(pli => pli.Material)
            .Include(pl => pl.PriceListItems)
                .ThenInclude(pli => pli.Labor)
            .Include(pl => pl.PriceListItems)
                .ThenInclude(pli => pli.Equipment)
            .FirstOrDefaultAsync(pl => pl.Id == priceListId);
    }
}

public class EstimateItemRepository : Repository<EstimateItem>, IEstimateItemRepository
{
    public EstimateItemRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<EstimateItem>> GetItemsByProjectAsync(int projectId)
    {
        return await _dbSet
            .Where(ei => ei.ProjectId == projectId)
            .Include(ei => ei.Material)
            .Include(ei => ei.Labor)
            .Include(ei => ei.Equipment)
            .Include(ei => ei.Standard)
            .OrderBy(ei => ei.SortOrder)
            .ToListAsync();
    }

    public async Task<IEnumerable<EstimateItem>> GetItemsByTypeAsync(EstimateItemType type)
    {
        return await _dbSet
            .Where(ei => ei.Type == type)
            .Include(ei => ei.Material)
            .Include(ei => ei.Labor)
            .Include(ei => ei.Equipment)
            .OrderBy(ei => ei.Name)
            .ToListAsync();
    }
}

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ConstructionEstimatorDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Category>> GetCategoriesByTypeAsync(string type)
    {
        return await _dbSet
            .Where(c => c.Type == type && c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetRootCategoriesAsync(string type)
    {
        return await _dbSet
            .Where(c => c.Type == type && c.ParentCategoryId == null && c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryWithSubCategoriesAsync(int categoryId)
    {
        return await _dbSet
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == categoryId);
    }
}