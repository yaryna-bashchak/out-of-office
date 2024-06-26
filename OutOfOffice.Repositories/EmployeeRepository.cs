﻿using Dapper;
using Microsoft.Data.SqlClient;
using OutOfOffice.Contracts.Models;
using OutOfOffice.Interfaces.Repositories;

namespace OutOfOffice.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly string _connectionString;

    public EmployeeRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Employee>> GetAllEmployeesAsync(string searchTerm = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT e.Id, e.FullName, e.OutOfOfficeBalance, e.PositionId, e.StatusId, e.SubdivisionId, e.PeoplePartnerId,
                   p.Id, p.Name,
                   s.Id, s.Name,
                   sub.Id, sub.Name,
                   pp.Id, pp.FullName
                FROM Employees e
                LEFT JOIN Positions p ON e.PositionId = p.Id
                LEFT JOIN EmployeeStatuses s ON e.StatusId = s.Id
                LEFT JOIN Subdivisions sub ON e.SubdivisionId = sub.Id
                LEFT JOIN Employees pp ON e.PeoplePartnerId = pp.Id";

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query += " WHERE e.FullName LIKE @SearchTerm";
            }

            var employees = await connection.QueryAsync<Employee, Position, EmployeeStatus, Subdivision, Employee, Employee>(
                query,
                (employee, position, status, subdivision, peoplePartner) =>
                {
                    employee.Position = position;
                    employee.Status = status;
                    employee.Subdivision = subdivision;
                    employee.PeoplePartner = peoplePartner;
                    return employee;
                },
                new { SearchTerm = $"%{searchTerm}%" },
                splitOn: "Id");

            return employees.ToList();
        }
    }

    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT e.Id, e.FullName, e.OutOfOfficeBalance, e.PositionId, e.StatusId, e.SubdivisionId, e.PeoplePartnerId,
                   p.ID as Id, p.Name,
                   s.ID as Id, s.Name,
                   sub.ID as Id, sub.Name,
                   pp.ID as Id, pp.FullName
                FROM Employees e
                LEFT JOIN Positions p ON e.PositionId = p.Id
                LEFT JOIN EmployeeStatuses s ON e.StatusId = s.Id
                LEFT JOIN Subdivisions sub ON e.SubdivisionId = sub.Id
                LEFT JOIN Employees pp ON e.PeoplePartnerId = pp.Id
                WHERE e.Id = @Id";

            var employee = await connection.QueryAsync<Employee, Position, EmployeeStatus, Subdivision, Employee, Employee>(
                query,
                (employee, position, status, subdivision, peoplePartner) =>
                {
                    employee.Position = position;
                    employee.Status = status;
                    employee.Subdivision = subdivision;
                    employee.PeoplePartner = peoplePartner;
                    return employee;
                },
                new { Id = id },
                splitOn: "Id");

            if (employee.FirstOrDefault() == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }

            return employee.FirstOrDefault();
        }
    }

    public async Task<Employee> AddEmployeeAsync(Employee employee)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                INSERT INTO Employees (Fullname, SubdivisionID, PositionID, StatusID, PeoplePartnerID, OutOfOfficeBalance)
                VALUES (@Fullname, @SubdivisionID, @PositionID, @StatusID, @PeoplePartnerID, @OutOfOfficeBalance);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            try
            {
                var employeeId = await connection.ExecuteScalarAsync<int>(query, employee);
                return await GetEmployeeByIdAsync(employeeId);
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Invalid foreign key. Please check the details and try again.");
            }
        }
    }

    public async Task<Employee> UpdateEmployeeAsync(Employee employee)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                UPDATE Employees
                SET Fullname = @Fullname, 
                    SubdivisionID = @SubdivisionID, 
                    PositionID = @PositionID,
                    StatusID = @StatusID,
                    PeoplePartnerID = @PeoplePartnerID, 
                    OutOfOfficeBalance = @OutOfOfficeBalance
                WHERE id = @Id";

            try
            {
                await connection.ExecuteAsync(query, employee);
                return await GetEmployeeByIdAsync(employee.Id);
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Invalid foreign key. Please check the details and try again.");
            }
        }
    }

    public async Task<List<int>> GetAllProjectManagerIdsOfEmployeeAsync(int employeeId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT p.ProjectManagerID
                FROM ProjectEmployees pe
                LEFT JOIN Employees e ON pe.EmployeeID = e.ID
                LEFT JOIN Projects p ON pe.ProjectID = p.ID
                WHERE e.ID = @Id;";

            var projectManagerIds = await connection.QueryAsync<int>(query, new { Id = employeeId });

            return projectManagerIds.ToList();
        }
    }

    public async Task<List<ProjectEmployee>> GetProjectEmployeesByProjectIdAsync(int projectId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT *
                FROM ProjectEmployees pe
                WHERE pe.ProjectId = @ProjectId";

            var employees = await connection.QueryAsync<ProjectEmployee>(query, new { ProjectId = projectId });
            return employees.ToList();
        }
    }

    public async Task<List<ProjectEmployee>> GetProjectEmployeesByEmployeeIdAsync(int employeeId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT *
                FROM ProjectEmployees pe
                WHERE pe.EmployeeId = @EmployeeId";

            var employees = await connection.QueryAsync<ProjectEmployee>(query, new { EmployeeId = employeeId });
            return employees.ToList();
        }
    }

    public async Task<List<Subdivision>> GetAllSubdivisionsAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT Id, Name
                FROM Subdivisions";

            var subdivisions = await connection.QueryAsync<Subdivision>(query);

            return subdivisions.ToList();
        }
    }

    public async Task<List<Position>> GetAllPositionsAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT Id, Name
                FROM Positions";

            var positions = await connection.QueryAsync<Position>(query);

            return positions.ToList();
        }
    }

    public async Task<List<EmployeeStatus>> GetAllStatusesAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT Id, Name
                FROM EmployeeStatuses";

            var statuses = await connection.QueryAsync<EmployeeStatus>(query);

            return statuses.ToList();
        }
    }

    public async Task<List<Employee>> GetAllEmployeesByPositionAsync(string positionName)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT e.Id, e.FullName, e.OutOfOfficeBalance, e.PositionId, e.StatusId, e.SubdivisionId, e.PeoplePartnerId,
                   p.Id, p.Name,
                   s.Id, s.Name,
                   sub.Id, sub.Name,
                   pp.Id, pp.FullName
                FROM Employees e
                LEFT JOIN Positions p ON e.PositionId = p.Id
                LEFT JOIN EmployeeStatuses s ON e.StatusId = s.Id
                LEFT JOIN Subdivisions sub ON e.SubdivisionId = sub.Id
                LEFT JOIN Employees pp ON e.PeoplePartnerId = pp.Id
                WHERE p.Name = @Position";

            var employees = await connection.QueryAsync<Employee, Position, EmployeeStatus, Subdivision, Employee, Employee>(
                query,
                (employee, position, status, subdivision, peoplePartner) =>
                {
                    employee.Position = position;
                    employee.Status = status;
                    employee.Subdivision = subdivision;
                    employee.PeoplePartner = peoplePartner;
                    return employee;
                },
                new { Position = positionName },
                splitOn: "Id");

            return employees.ToList();
        }
    }
}

