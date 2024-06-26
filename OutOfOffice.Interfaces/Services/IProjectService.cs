using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Contracts.DTOs.Project;
using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Services;

public interface IProjectService
{
    Task<List<GetProjectDto>> GetAllProjectsAsync(string searchTerm = null);
    Task<GetProjectDto> GetProjectByIdAsync(int id);
    Task<GetProjectDto> AddProjectAsync(AddProjectDto projectDto);
    Task<GetProjectDto> UpdateProjectAsync(int id, UpdateProjectDto projectDto);
    Task AddEmployeeToProjectAsync(ProjectEmployeeDto projectEmployee);
    Task UpdateEmployeeInProjectAsync(ProjectEmployeeDto projectEmployee);
    Task<List<ProjectType>> GetAllTypesAsync();
    Task<List<ProjectStatus>> GetAllStatusesAsync();
}
