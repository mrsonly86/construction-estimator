using AutoMapper;
using Microsoft.Extensions.Logging;
using ConstructionEstimator.Core.Interfaces;
using ConstructionEstimator.Core.Models;
using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Enums;
using ConstructionEstimator.Services.Interfaces;

namespace ConstructionEstimator.Services.Implementations;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ProjectService> _logger;
    private readonly ICalculationService _calculationService;

    public ProjectService(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ILogger<ProjectService> logger,
        ICalculationService calculationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _calculationService = calculationService;
    }

    public async Task<ProjectDto> CreateProjectAsync(ProjectDto projectDto)
    {
        try
        {
            var project = _mapper.Map<Project>(projectDto);
            project.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Created new project: {ProjectName} with ID: {ProjectId}", project.Name, project.Id);
            
            return _mapper.Map<ProjectDto>(project);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project: {ProjectName}", projectDto.Name);
            throw;
        }
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(int id)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(id);
        return project != null ? _mapper.Map<ProjectDto>(project) : null;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = await _unitOfWork.Projects.GetAllAsync();
        return _mapper.Map<IEnumerable<ProjectDto>>(projects);
    }

    public async Task<IEnumerable<ProjectDto>> GetProjectsByClientAsync(string clientName)
    {
        var projects = await _unitOfWork.Projects.GetProjectsByClientAsync(clientName);
        return _mapper.Map<IEnumerable<ProjectDto>>(projects);
    }

    public async Task<IEnumerable<ProjectDto>> GetProjectsByStatusAsync(ProjectStatus status)
    {
        var projects = await _unitOfWork.Projects.GetProjectsByStatusAsync(status);
        return _mapper.Map<IEnumerable<ProjectDto>>(projects);
    }

    public async Task<ProjectDto> UpdateProjectAsync(ProjectDto projectDto)
    {
        try
        {
            var existingProject = await _unitOfWork.Projects.GetByIdAsync(projectDto.Id);
            if (existingProject == null)
            {
                throw new ArgumentException($"Project with ID {projectDto.Id} not found");
            }

            _mapper.Map(projectDto, existingProject);
            existingProject.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Projects.UpdateAsync(existingProject);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Updated project: {ProjectName} with ID: {ProjectId}", existingProject.Name, existingProject.Id);
            
            return _mapper.Map<ProjectDto>(existingProject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating project with ID: {ProjectId}", projectDto.Id);
            throw;
        }
    }

    public async Task DeleteProjectAsync(int id)
    {
        try
        {
            await _unitOfWork.Projects.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Deleted project with ID: {ProjectId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting project with ID: {ProjectId}", id);
            throw;
        }
    }

    public async Task<ProjectDto?> GetProjectWithItemsAsync(int projectId)
    {
        var project = await _unitOfWork.Projects.GetProjectWithItemsAsync(projectId);
        return project != null ? _mapper.Map<ProjectDto>(project) : null;
    }

    public async Task<decimal> CalculateProjectTotalAsync(int projectId)
    {
        var project = await _unitOfWork.Projects.GetProjectWithItemsAsync(projectId);
        if (project == null) return 0;

        var materialCost = project.EstimateItems.Where(i => i.Type == EstimateItemType.Material).Sum(i => i.TotalPrice);
        var laborCost = project.EstimateItems.Where(i => i.Type == EstimateItemType.Labor).Sum(i => i.TotalPrice);
        var equipmentCost = project.EstimateItems.Where(i => i.Type == EstimateItemType.Equipment).Sum(i => i.TotalPrice);

        var breakdown = _calculationService.CalculateCostBreakdown(
            materialCost, laborCost, equipmentCost, 
            project.ProfitPercentage, project.ContingencyPercentage);

        return breakdown["TotalCost"];
    }
}