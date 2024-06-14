using Dapper;
using Microsoft.Data.SqlClient;
using OutOfOffice.Contracts.Models;
using OutOfOffice.Interfaces.Repositories;

namespace OutOfOffice.Repositories;

public class ApprovalRequestRepository : IApprovalRequestRepository
{
    private readonly string _connectionString;
    private readonly ILeaveRequestRepository _leaveRequestRepository;

    public ApprovalRequestRepository(string connectionString)
    {
        _connectionString = connectionString;
        _leaveRequestRepository = new LeaveRequestRepository(_connectionString);
    }

    public async Task<List<ApprovalRequest>> GetAllApprovalRequestsAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT ar.Id, ar.Comment, ar.LeaveRequestId, ar.ApproverID, ar.StatusId,
                    e.Id, e.FullName,
                    s.Id, s.Name
                FROM ApprovalRequests ar
                LEFT JOIN Employees e ON ar.ApproverID = e.Id
                LEFT JOIN ApprovalRequestStatuses s ON ar.StatusId = s.Id";

            var approvalRequests = await connection.QueryAsync<ApprovalRequest, Employee, ApprovalRequestStatus, ApprovalRequest>(
                query,
                (approvalRequest, approver, approvalRequestStatus) =>
                {
                    approvalRequest.Approver = approver;
                    approvalRequest.Status = approvalRequestStatus;
                    return approvalRequest;
                },
                splitOn: "Id");

            foreach (var approvalRequest in approvalRequests)
            {
                approvalRequest.LeaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(approvalRequest.LeaveRequestId);
            }

            return approvalRequests.ToList();
        }
    }

    public async Task<ApprovalRequest> GetApprovalRequestByIdAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT ar.Id, ar.Comment, ar.LeaveRequestId, ar.ApproverID, ar.StatusId,
                    e.Id, e.FullName,
                    s.Id, s.Name
                FROM ApprovalRequests ar
                LEFT JOIN Employees e ON ar.ApproverID = e.Id
                LEFT JOIN ApprovalRequestStatuses s ON ar.StatusId = s.Id
                WHERE ar.Id = @Id";

            var approvalRequests = await connection.QueryAsync<ApprovalRequest, Employee, ApprovalRequestStatus, ApprovalRequest>(
                query,
                (approvalRequest, approver, approvalRequestStatus) =>
                {
                    approvalRequest.Approver = approver;
                    approvalRequest.Status = approvalRequestStatus;
                    return approvalRequest;
                },
                new { Id = id },
                splitOn: "Id");

            var approvalRequest = approvalRequests.FirstOrDefault();
            if (approvalRequest == null)
            {
                throw new KeyNotFoundException($"ApprovalRequest with ID {id} not found.");
            }

            approvalRequest.LeaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(approvalRequest.LeaveRequestId);
            return approvalRequest;
        }
    }

    public async Task<ApprovalRequest> AddApprovalRequestAsync(ApprovalRequest approvalRequest)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                INSERT INTO ApprovalRequests (ApproverID, LeaveRequestID, StatusID, Comment)
                VALUES (@ApproverID, @LeaveRequestID, @StatusID, @Comment);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            try
            {
                var approvalRequestId = await connection.ExecuteScalarAsync<int>(query, approvalRequest);
                return await GetApprovalRequestByIdAsync(approvalRequestId);
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Invalid foreign key. Please check the details and try again.");
            }
        }
    }

    public async Task<ApprovalRequest> UpdateApprovalRequestAsync(ApprovalRequest approvalRequest)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                UPDATE ApprovalRequests
                SET ApproverID = @ApproverID,
                    LeaveRequestID = @LeaveRequestID,
                    StatusID = @StatusID,
                    Comment = @Comment
                WHERE id = @id";

            try
            {
                await connection.ExecuteAsync(query, approvalRequest);
                return await GetApprovalRequestByIdAsync(approvalRequest.Id);
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException("Invalid foreign key. Please check the details and try again.");
            }
        }
    }

    public async Task<List<ApprovalRequestStatus>> GetAllStatusesAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT Id, Name
                FROM ApprovalRequestStatuses";

            var statuses = await connection.QueryAsync<ApprovalRequestStatus>(query);

            return statuses.ToList();
        }
    }

    public async Task<List<ApprovalRequest>> GetAllApprovalRequestsByLeaveRequestIdAsync(int leaveRequestId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = @"
                SELECT ar.*, s.Id, s.Name
                FROM ApprovalRequests ar
                    LEFT JOIN ApprovalRequestStatuses s ON ar.StatusId = s.Id
                WHERE ar.LeaveRequestID = @LeaveRequestId";

            var approvalRequests = await connection.QueryAsync<ApprovalRequest, ApprovalRequestStatus, ApprovalRequest>(
                query,
                (approvalRequest, approvalRequestStatus) =>
                {
                    approvalRequest.Status = approvalRequestStatus;
                    return approvalRequest;
                },
                new { LeaveRequestId = leaveRequestId },
                splitOn: "Id");
            return approvalRequests.ToList();
        }
    }
}

