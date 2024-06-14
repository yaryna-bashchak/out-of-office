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
                SELECT ar.Id, ar.Comment, ar.LeaveRequestId,
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
        throw new NotImplementedException();

    }

    public async Task<ApprovalRequest> AddApprovalRequestAsync(ApprovalRequest approvalRequest)
    {
        throw new NotImplementedException();

    }

    public async Task<ApprovalRequest> UpdateApprovalRequestAsync(ApprovalRequest approvalRequest)
    {
        throw new NotImplementedException();

    }
}

