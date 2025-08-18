using ConstructionEstimator.Core.Entities;

namespace ConstructionEstimator.Core.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}

public interface IProjectRepository : IRepository<Project>
{
    Task<IEnumerable<Project>> GetProjectsByStatusAsync(Enums.ProjectStatus status);
    Task<Project?> GetProjectWithSectionsAsync(int projectId);
}

public interface IMaterialRepository : IRepository<Material>
{
    Task<IEnumerable<Material>> GetMaterialsByCategoryAsync(string category);
    Task<IEnumerable<Material>> SearchMaterialsAsync(string searchTerm);
}

public interface ILaborRepository : IRepository<Labor>
{
    Task<IEnumerable<Labor>> GetLaborsByCategoryAsync(string category);
    Task<IEnumerable<Labor>> GetLaborsBySkillLevelAsync(Enums.LaborSkillLevel skillLevel);
}