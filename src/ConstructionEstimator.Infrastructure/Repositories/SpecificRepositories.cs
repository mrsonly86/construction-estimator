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