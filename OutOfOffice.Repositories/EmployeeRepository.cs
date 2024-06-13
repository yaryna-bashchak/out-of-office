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

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT e.Id, e.FullName, e.OutOfOfficeBalance, e.Photo,
                   p.Id, p.Name,
                   s.Id, s.Name,
                   sub.Id, sub.Name,
                   pp.Id, pp.FullName
                FROM Employees e
                LEFT JOIN Positions p ON e.PositionId = p.Id
                LEFT JOIN EmployeeStatuses s ON e.StatusId = s.Id
                LEFT JOIN Subdivisions sub ON e.SubdivisionId = sub.Id
                LEFT JOIN Employees pp ON e.PeoplePartnerId = pp.Id";

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
                SELECT e.Id, e.FullName, e.OutOfOfficeBalance, e.Photo,
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

            return employee.FirstOrDefault();
        }
    }

    public async Task<Employee> AddEmployeeAsync(Employee employee)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                INSERT INTO Employees (Fullname, SubdivisionID, PositionID, StatusID, PeoplePartnerID, OutOfOfficeBalance, Photo)
                VALUES (@Fullname, @SubdivisionID, @PositionID, @StatusID, @PeoplePartnerID, @OutOfOfficeBalance, @Photo);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            var employeeId = await connection.ExecuteScalarAsync<int>(query, employee);

            if (employeeId == 0)
                return null;

            return await GetEmployeeByIdAsync(employeeId);
        }
    }

    public async Task<Employee> UpdateEmployeeAsync(Employee updatedEmployee)
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
                    OutOfOfficeBalance = @OutOfOfficeBalance, 
                    Photo = @Photo
                WHERE id = @id";

            var affectedRows = await connection.ExecuteAsync(query, updatedEmployee);

            if (affectedRows == 0)
                return null;

            return await GetEmployeeByIdAsync(updatedEmployee.Id);
        }
    }
}

