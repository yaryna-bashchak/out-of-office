using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetAllProjects();
    Task<Project> GetProjectById(int id);
    Task<Project> AddProject(Project newProject);
    Task<Project> UpdateProject(Project updatedProject);
    Task<bool> DeleteProject(int id);
    Task<Project> AddEmployeeToProject(ProjectEmployee newProjectEmployee);
    Task<Project> UpdateEmployeeInProject(ProjectEmployee updatedProjectEmployee);
    Task<bool> DeleteEmployeeFromProject(int projectId, int employeeId);
}
