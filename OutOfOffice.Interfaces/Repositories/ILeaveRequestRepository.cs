using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Interfaces.Repositories;

public interface ILeaveRequestRepository
{
    Task<List<LeaveRequest>> GetAllLeaveRequestsAsync();
    Task<LeaveRequest> GetLeaveRequestByIdAsync(int id);
    Task<LeaveRequest> AddLeaveRequestAsync(LeaveRequest leaveRequest);
    Task<LeaveRequest> UpdateLeaveRequestAsync(LeaveRequest leaveRequest);
    Task<List<AbsenceReason>> GetAllAbsenceReasonsAsync();
    Task<List<RequestType>> GetAllRequestTypesAsync();
    Task<List<LeaveRequestStatus>> GetAllStatusesAsync();
}
