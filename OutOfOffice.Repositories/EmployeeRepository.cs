using Dapper;
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

    public async Task<List<Employee>> GetAllAsync()
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

    public async Task<Employee> GetByIdAsync(int id)
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

    public Task<Employee> AddAsync(Employee newObject)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Employee> UpdateAsync(Employee updatedObject)
    {
        throw new NotImplementedException();
    }
}

