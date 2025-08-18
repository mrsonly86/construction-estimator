using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Interfaces;

namespace ConstructionEstimator.Core.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Project> CreateProjectAsync(Project project)
    {
        project.CreatedDate = DateTime.Now;
        await _projectRepository.AddAsync(project);
        await _projectRepository.SaveChangesAsync();
        return project;
    }

    public async Task<Project?> GetProjectAsync(int id)
    {
        return await _projectRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        return await _projectRepository.GetAllAsync();
    }

    public async Task<Project> UpdateProjectAsync(Project project)
    {
        project.UpdatedDate = DateTime.Now;
        await _projectRepository.UpdateAsync(project);
        await _projectRepository.SaveChangesAsync();
        return project;
    }

    public async Task DeleteProjectAsync(int id)
    {
        await _projectRepository.DeleteAsync(id);
        await _projectRepository.SaveChangesAsync();
    }

    public async Task<Project?> GetProjectWithDetailsAsync(int id)
    {
        return await _projectRepository.GetProjectWithSectionsAsync(id);
    }
}