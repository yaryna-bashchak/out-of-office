using Dapper;
using Microsoft.Data.SqlClient;
using OutOfOffice.Contracts.Models;
using OutOfOffice.Interfaces.Repositories;

namespace OutOfOffice.Repositories;

public class LeaveRequestRepository : ILeaveRequestRepository
{
    private readonly string _connectionString;

    public LeaveRequestRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<LeaveRequest>> GetAllLeaveRequestsAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT lr.Id, lr.StartDate, lr.EndDate, lr.Hours, lr.Comment, lr.EmployeeId, lr.AbsenceReasonId, lr.RequestTypeId, lr.StatusId,
                   e.Id, e.FullName,
                   ar.Id, ar.Name,
                   rt.Id, rt.Name,
                   s.Id, s.Name
                FROM LeaveRequests lr
                LEFT JOIN Employees e ON lr.EmployeeId = e.Id
                LEFT JOIN AbsenceReasons ar ON lr.AbsenceReasonId = ar.Id
                LEFT JOIN RequestTypes rt ON lr.RequestTypeId = rt.Id
                LEFT JOIN LeaveRequestStatuses s ON lr.StatusId = s.Id";

            var leaveRequests = await connection.QueryAsync<LeaveRequest, Employee, AbsenceReason, RequestType, LeaveRequestStatus, LeaveRequest>(
                query,
                (leaveRequest, employee, absenceReason, requestType, status) =>
                {
                    leaveRequest.Employee = employee;
                    leaveRequest.AbsenceReason = absenceReason;
                    leaveRequest.RequestType = requestType;
                    leaveRequest.Status = status;
                    return leaveRequest;
                },
                splitOn: "Id");

            return leaveRequests.ToList();
        }
    }

    public async Task<LeaveRequest> GetLeaveRequestByIdAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT lr.Id, lr.StartDate, lr.EndDate, lr.Hours, lr.Comment, lr.EmployeeId, lr.AbsenceReasonId, lr.RequestTypeId, lr.StatusId,
                   e.Id, e.FullName,
                   ar.Id, ar.Name,
                   rt.Id, rt.Name,
                   s.Id, s.Name
                FROM LeaveRequests lr
                LEFT JOIN Employees e ON lr.EmployeeId = e.Id
                LEFT JOIN AbsenceReasons ar ON lr.AbsenceReasonId = ar.Id
                LEFT JOIN RequestTypes rt ON lr.RequestTypeId = rt.Id
                LEFT JOIN LeaveRequestStatuses s ON lr.StatusId = s.Id
                WHERE lr.Id = @Id";

            var leaveRequest = await connection.QueryAsync<LeaveRequest, Employee, AbsenceReason, RequestType, LeaveRequestStatus, LeaveRequest>(
                query,
                (leaveRequest, employee, absenceReason, requestType, status) =>
                {
                    leaveRequest.Employee = employee;
                    leaveRequest.AbsenceReason = absenceReason;
                    leaveRequest.RequestType = requestType;
                    leaveRequest.Status = status;
                    return leaveRequest;
                },
                new { Id = id },
                splitOn: "Id");

            if (leaveRequest.FirstOrDefault() == null)
            {
                throw new KeyNotFoundException($"LeaveRequest with ID {id} not found.");
            }

            return leaveRequest.FirstOrDefault();
        }
    }

    public async Task<LeaveRequest> AddLeaveRequestAsync(LeaveRequest leaveRequest)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                INSERT INTO LeaveRequests (EmployeeId, AbsenceReasonID, StartDate, EndDate, RequestTypeID, Hours, StatusID, Comment)
                VALUES (@EmployeeId, @AbsenceReasonID, @StartDate, @EndDate, @RequestTypeID, @Hours, @StatusID, @Comment);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            try
            {
                var leaveRequestId = await connection.ExecuteScalarAsync<int>(query, leaveRequest);
                return await GetLeaveRequestByIdAsync(leaveRequestId);
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Invalid foreign key. Please check the details and try again.");
            }
        }
    }

    public async Task<LeaveRequest> UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                UPDATE LeaveRequests
                SET EmployeeId = @EmployeeId,
                    AbsenceReasonID = @AbsenceReasonID,
                    StartDate = @StartDate,
                    EndDate = @EndDate,
                    RequestTypeID = @RequestTypeID,
                    Hours = @Hours,
                    StatusID = @StatusID,
                    Comment = @Comment
                WHERE id = @Id";

            try
            {
                await connection.ExecuteAsync(query, leaveRequest);
                return await GetLeaveRequestByIdAsync(leaveRequest.Id);
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Invalid foreign key. Please check the details and try again.");
            }
        }
    }

    public async Task<List<AbsenceReason>> GetAllAbsenceReasonsAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT Id, Name
                FROM AbsenceReasons";

            var absenceReasons = await connection.QueryAsync<AbsenceReason>(query);

            return absenceReasons.ToList();
        }
    }

    public async Task<List<RequestType>> GetAllRequestTypesAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT Id, Name
                FROM RequestTypes";

            var requestTypes = await connection.QueryAsync<RequestType>(query);

            return requestTypes.ToList();
        }
    }

    public async Task<List<LeaveRequestStatus>> GetAllStatusesAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT Id, Name
                FROM LeaveRequestStatuses";

            var statuses = await connection.QueryAsync<LeaveRequestStatus>(query);

            return statuses.ToList();
        }
    }
}

