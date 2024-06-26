using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Contracts.DTOs.Project;
using OutOfOffice.Contracts.Models;
using OutOfOffice.Interfaces.Repositories;
using OutOfOffice.Interfaces.Services;

namespace OutOfOffice.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public ProjectService(IProjectRepository projectRepository, IEmployeeRepository employeeRepository)
    {
        _projectRepository = projectRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<List<GetProjectDto>> GetAllProjectsAsync(string searchTerm = null)
    {
        var projects = await _projectRepository.GetAllProjectsAsync(searchTerm);
        var projectDtos = new List<GetProjectDto>();

        foreach (var project in projects)
        {
            var projectDto = CustomMapper.MapToProjectDto(project);
            var projectMembers = await _employeeRepository.GetProjectEmployeesByProjectIdAsync(project.Id);
            foreach (var member in projectMembers)
            {
                projectDto.ProjectEmployees.Add(CustomMapper.MapToProjectEmployeeDto(member));
            }

            projectDtos.Add(projectDto);
        }

        return projectDtos;
    }

    public async Task<GetProjectDto> GetProjectByIdAsync(int id)
    {
        var project = await _projectRepository.GetProjectByIdAsync(id);
        var projectDto = CustomMapper.MapToProjectDto(project);

        var projectMembers = await _employeeRepository.GetProjectEmployeesByProjectIdAsync(id);
        foreach (var member in projectMembers)
        {
            projectDto.ProjectEmployees.Add(CustomMapper.MapToProjectEmployeeDto(member));
        }

        return projectDto;
    }

    public async Task<GetProjectDto> AddProjectAsync(AddProjectDto projectDto)
    {
        var project = CustomMapper.MapToProject(projectDto);
        var addedProject = await _projectRepository.AddProjectAsync(project);
        return CustomMapper.MapToProjectDto(addedProject);
    }

    public async Task<GetProjectDto> UpdateProjectAsync(int id, UpdateProjectDto projectDto)
    {
        var project = CustomMapper.MapToProject(id, projectDto);
        var updatedProject = await _projectRepository.UpdateProjectAsync(project);
        return CustomMapper.MapToProjectDto(updatedProject);
    }

    public async Task AddEmployeeToProjectAsync(ProjectEmployeeDto projectEmployeeDto)
    {
        var projectEmployee = CustomMapper.MapToProjectEmployee(projectEmployeeDto);
        await _projectRepository.AddEmployeeToProjectAsync(projectEmployee);
    }

    public async Task UpdateEmployeeInProjectAsync(ProjectEmployeeDto projectEmployeeDto)
    {
        var projectEmployee = CustomMapper.MapToProjectEmployee(projectEmployeeDto);
        await _projectRepository.UpdateEmployeeInProjectAsync(projectEmployee);
    }

    public async Task<List<ProjectType>> GetAllTypesAsync()
    {
        return await _projectRepository.GetAllTypesAsync();
    }

    public async Task<List<ProjectStatus>> GetAllStatusesAsync()
    {
        return await _projectRepository.GetAllStatusesAsync();
    }
}
