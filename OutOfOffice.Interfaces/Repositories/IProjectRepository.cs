using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetAllProjectsAsync(string searchTerm = null);
    Task<Project> GetProjectByIdAsync(int id);
    Task<Project> AddProjectAsync(Project project);
    Task<Project> UpdateProjectAsync(Project project);
    Task AddEmployeeToProjectAsync(ProjectEmployee newProjectEmployee);
    Task UpdateEmployeeInProjectAsync(ProjectEmployee updatedProjectEmployee);
    Task<List<ProjectType>> GetAllTypesAsync();
    Task<List<ProjectStatus>> GetAllStatusesAsync();
}
