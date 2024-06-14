using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetAllProjectsAsync();
    Task<Project> GetProjectByIdAsync(int id);
    Task<Project> AddProjectAsync(Project project);
    Task<Project> UpdateProjectAsync(Project project);
    Task<Project> AddEmployeeToProjectAsync(ProjectEmployee newProjectEmployee);
    Task<Project> UpdateEmployeeInProjectAsync(ProjectEmployee updatedProjectEmployee);
}
