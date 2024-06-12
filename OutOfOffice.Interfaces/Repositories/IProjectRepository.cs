using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetAllProjectsAsync();
    Task<Project> GetProjectByIdAsync(int id);
    Task<Project> AddProjectAsync(Project newProject);
    Task<Project> UpdateProjectAsync(Project updatedProject);
    Task<bool> DeleteProjectAsync(int id);
    Task<Project> AddEmployeeToProjectAsync(ProjectEmployee newProjectEmployee);
    Task<Project> UpdateEmployeeInProjectAsync(ProjectEmployee updatedProjectEmployee);
    Task<bool> DeleteEmployeeFromProjectAsync(int projectId, int employeeId);
}
