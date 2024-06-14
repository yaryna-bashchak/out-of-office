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
        var approvalRequest = await _approvalRequestRepository.GetApprovalRequestByIdAsync(id);
        return MapToApprovalRequestDto(approvalRequest);
    }

    public async Task<GetApprovalRequestDto> UpdateApprovalRequestAsync(int id, UpdateApprovalRequestDto approvalRequest)
    {
        // add changing status of leave request

        var mappedApprovalRequest = MapToApprovalRequest(id, approvalRequest);
        var updatedApprovalRequest = await _approvalRequestRepository.UpdateApprovalRequestAsync(mappedApprovalRequest);
        return MapToApprovalRequestDto(updatedApprovalRequest);
    }

    private static GetApprovalRequestDto MapToApprovalRequestDto(ApprovalRequest approvalRequest)
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
    
    private static ApprovalRequest MapToApprovalRequest(int id, UpdateApprovalRequestDto approvalRequest)
    {
        return new ApprovalRequest
        {
            Id = id,
            Comment = approvalRequest.Comment,
            ApproverId = approvalRequest.ApproverId,
            LeaveRequestId = approvalRequest.LeaveRequestId,
            StatusId = approvalRequest.StatusId,
        };
    }
}
