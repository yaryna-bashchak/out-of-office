using Dapper;
using Microsoft.Data.SqlClient;
using OutOfOffice.Contracts.Models;
using OutOfOffice.Interfaces.Repositories;

namespace OutOfOffice.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly string _connectionString;
    private readonly IEmployeeRepository _employeeRepository;

    public ProjectRepository(string connectionString)
    {
        _connectionString = connectionString;
        _employeeRepository = new EmployeeRepository(_connectionString);
    }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT p.*, pt.ID, pt.Name, ps.ID, ps.Name
                FROM Projects p
                LEFT JOIN ProjectTypes pt ON p.ProjectTypeID = pt.ID
                LEFT JOIN ProjectStatuses ps ON p.StatusID = ps.ID";

            var projects = await connection.QueryAsync<Project, ProjectType, ProjectStatus, Project>(
                query,
                (project, type, status) =>
                {
                    project.ProjectType = type;
                    project.Status = status;
                    return project;
                },
                splitOn: "Id");

            foreach (var project in projects)
            {
                project.ProjectManager = await _employeeRepository.GetEmployeeByIdAsync(project.ProjectManagerId);
            }

            return projects.ToList();
        }
    }

    public async Task<Project> GetProjectByIdAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT p.*, pt.ID, pt.Name, ps.ID, ps.Name
                FROM Projects p
                LEFT JOIN ProjectTypes pt ON p.ProjectTypeID = pt.ID
                LEFT JOIN ProjectStatuses ps ON p.StatusID = ps.ID
                WHERE p.Id = @Id";

            var projects = await connection.QueryAsync<Project, ProjectType, ProjectStatus, Project>(
                query,
                (project, type, status) =>
                {
                    project.ProjectType = type;
                    project.Status = status;
                    return project;
                },
                new { Id = id },
                splitOn: "Id");

            var project = projects.FirstOrDefault();

            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {id} not found.");
            }

            project.ProjectManager = await _employeeRepository.GetEmployeeByIdAsync(project.ProjectManagerId);
            return project;
        }
    }

    public async Task<Project> AddProjectAsync(Project project)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                INSERT INTO Projects (ProjectTypeId, StartDate, EndDate, ProjectManagerId, StatusId, Comment)
                VALUES (@ProjectTypeId, @StartDate, @EndDate, @ProjectManagerId, @StatusId, @Comment);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            try
            {
                var projectId = await connection.ExecuteScalarAsync<int>(query, project);
                return await GetProjectByIdAsync(projectId);
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Invalid foreign key. Please check the details and try again.");
            }
        }
    }

    public async Task<Project> UpdateProjectAsync(Project project)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                UPDATE Projects
                SET ProjectTypeId = @ProjectTypeId, 
                    StartDate = @StartDate, 
                    EndDate = @EndDate, 
                    ProjectManagerId = @ProjectManagerId, 
                    StatusId = @StatusId, 
                    Comment = @Comment
                WHERE Id = @Id";

            try
            {
                await connection.ExecuteAsync(query, project);
                return await GetProjectByIdAsync(project.Id);
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Invalid foreign key. Please check the details and try again.");
            }
        }
    }

    public Task<Project> AddEmployeeToProjectAsync(ProjectEmployee newProjectEmployee)
    {
        throw new NotImplementedException();
    }

    public Task<Project> UpdateEmployeeInProjectAsync(ProjectEmployee updatedProjectEmployee)
    {
        throw new NotImplementedException();
    }
}
