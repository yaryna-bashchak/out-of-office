using OutOfOffice.Contracts.DTOs;

namespace OutOfOffice.Interfaces.Services;

public interface IProjectService
{
    Task<List<GetProjectDto>> GetAllProjectsAsync();
    Task<GetProjectDto> GetProjectByIdAsync(int id);
    Task<GetProjectDto> AddProjectAsync(AddProjectDto projectDto);
    Task<GetProjectDto> UpdateProjectAsync(int id, UpdateProjectDto projectDto);
}
