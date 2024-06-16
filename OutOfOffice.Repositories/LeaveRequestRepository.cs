using Dapper;
using Microsoft.Data.SqlClient;
using OutOfOffice.Contracts.Models;
using OutOfOffice.Interfaces.Repositories;

namespace OutOfOffice.Repositories;

public class LeaveRequestRepository : ILeaveRequestRepository
{
    private readonly string _connectionString;
    private readonly IEmployeeRepository _employeeRepository;

    public LeaveRequestRepository(string connectionString)
    {
        _connectionString = connectionString;
        _employeeRepository = new EmployeeRepository(_connectionString);
    }

    public async Task<List<LeaveRequest>> GetAllLeaveRequestsAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT lr.Id, lr.StartDate, lr.EndDate, lr.Hours, lr.Comment, lr.EmployeeId, lr.AbsenceReasonId, lr.RequestTypeId, lr.StatusId,
                   ar.Id, ar.Name,
                   rt.Id, rt.Name,
                   s.Id, s.Name
                FROM LeaveRequests lr
                LEFT JOIN AbsenceReasons ar ON lr.AbsenceReasonId = ar.Id
                LEFT JOIN RequestTypes rt ON lr.RequestTypeId = rt.Id
                LEFT JOIN LeaveRequestStatuses s ON lr.StatusId = s.Id";

            var leaveRequests = await connection.QueryAsync<LeaveRequest, AbsenceReason, RequestType, LeaveRequestStatus, LeaveRequest>(
                query,
                (leaveRequest, absenceReason, requestType, status) =>
                {
                    leaveRequest.AbsenceReason = absenceReason;
                    leaveRequest.RequestType = requestType;
                    leaveRequest.Status = status;
                    return leaveRequest;
                },
                splitOn: "Id");

            foreach (var leaveRequest in leaveRequests)
            {
                leaveRequest.Employee = await _employeeRepository.GetEmployeeByIdAsync(leaveRequest.EmployeeId);
            }

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
                   ar.Id, ar.Name,
                   rt.Id, rt.Name,
                   s.Id, s.Name
                FROM LeaveRequests lr
                LEFT JOIN AbsenceReasons ar ON lr.AbsenceReasonId = ar.Id
                LEFT JOIN RequestTypes rt ON lr.RequestTypeId = rt.Id
                LEFT JOIN LeaveRequestStatuses s ON lr.StatusId = s.Id
                WHERE lr.Id = @Id";

            var leaveRequests = await connection.QueryAsync<LeaveRequest, AbsenceReason, RequestType, LeaveRequestStatus, LeaveRequest>(
                query,
                (leaveRequest, absenceReason, requestType, status) =>
                {
                    leaveRequest.AbsenceReason = absenceReason;
                    leaveRequest.RequestType = requestType;
                    leaveRequest.Status = status;
                    return leaveRequest;
                },
                new { Id = id },
                splitOn: "Id");

            var leaveRequest = leaveRequests.FirstOrDefault();
            if (leaveRequest == null)
            {
                throw new KeyNotFoundException($"LeaveRequest with ID {id} not found.");
            }

            leaveRequest.Employee = await _employeeRepository.GetEmployeeByIdAsync(leaveRequest.EmployeeId);

            return leaveRequest;
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

    public async Task<List<RequestType>> GetAllTypesAsync()
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

