using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Contracts.Models;
using OutOfOffice.Interfaces.Repositories;
using OutOfOffice.Interfaces.Services;

namespace OutOfOffice.Services;

public class ApprovalRequestService : IApprovalRequestService
{
    private readonly IApprovalRequestRepository _approvalRequestRepository;
    public ApprovalRequestService(IApprovalRequestRepository approvalRequestRepository)
    {
        _approvalRequestRepository = approvalRequestRepository;

    }
    public async Task<List<GetApprovalRequestDto>> GetAllApprovalRequestsAsync()
    {
        var approvalRequests = await _approvalRequestRepository.GetAllApprovalRequestsAsync();
        return approvalRequests.Select(ar => MapToApprovalRequestDto(ar)).ToList();
    }

    public async Task<GetApprovalRequestDto> GetApprovalRequestByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<GetApprovalRequestDto> UpdateApprovalRequestAsync(int id, UpdateApprovalRequestDto approvalRequest)
    {
        throw new NotImplementedException();
    }

    private GetApprovalRequestDto MapToApprovalRequestDto(ApprovalRequest approvalRequest)
    {
        return new GetApprovalRequestDto
        {
            Id = approvalRequest.Id,
            Comment = approvalRequest.Comment,
            Approver = new GetEmployeeDto { Id = approvalRequest.Approver.Id, FullName = approvalRequest.Approver.FullName },
            LeaveRequest = LeaveRequestService.MapToLeaveRequestDto(approvalRequest.LeaveRequest),
            Status = approvalRequest.Status,
        };
    }
}
