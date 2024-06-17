using OutOfOffice.Contracts.DTOs;
using OutOfOffice.Contracts.DTOs.Project;
using OutOfOffice.Contracts.Models;

namespace OutOfOffice.Services;

public static class CustomMapper
{

    public static GetEmployeeDto MapToEmployeeDto(Employee employee)
    {
        return new GetEmployeeDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            OutOfOfficeBalance = employee.OutOfOfficeBalance,
            Photo = employee.Photo,
            PeoplePartner = employee.PeoplePartner != null ? MapToPeoplePartnerDto(employee.PeoplePartner) : null,
            Position = employee.Position,
            Status = employee.Status,
            Subdivision = employee.Subdivision,
        };
    }

    public static Employee MapToEmployee(AddEmployeeDto employeeDto)
    {
        return new Employee
        {
            FullName = employeeDto.FullName,
            OutOfOfficeBalance = employeeDto.OutOfOfficeBalance,
            // Photo = employeeDto.Photo,
            PeoplePartnerId = employeeDto.PeoplePartnerId,
            PositionId = employeeDto.PositionId,
            StatusId = employeeDto.StatusId,
            SubdivisionId = employeeDto.SubdivisionId,
        };
    }

    public static Employee MapToEmployee(int id, UpdateEmployeeDto employeeDto)
    {
        return new Employee
        {
            Id = id,
            FullName = employeeDto.FullName,
            OutOfOfficeBalance = employeeDto.OutOfOfficeBalance,
            // Photo = employeeDto.Photo,
            PeoplePartnerId = employeeDto.PeoplePartnerId,
            PositionId = employeeDto.PositionId,
            StatusId = employeeDto.StatusId,
            SubdivisionId = employeeDto.SubdivisionId,
        };
    }

    public static PeoplePartnerDto MapToPeoplePartnerDto(Employee employee)
    {
        return new PeoplePartnerDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
        };
    }

    public static GetLeaveRequestDto MapToLeaveRequestDto(LeaveRequest leaveRequest)
    {
        return new GetLeaveRequestDto
        {
            Id = leaveRequest.Id,
            StartDate = leaveRequest.StartDate,
            EndDate = leaveRequest.EndDate,
            Hours = leaveRequest.Hours,
            Comment = leaveRequest.Comment,
            Employee = MapToEmployeeDto(leaveRequest.Employee),
            AbsenceReason = leaveRequest.AbsenceReason,
            RequestType = leaveRequest.RequestType,
            Status = leaveRequest.Status,
        };
    }

    public static LeaveRequest MapToLeaveRequest(AddLeaveRequestDto leaveRequestDto)
    {
        return new LeaveRequest
        {
            StartDate = leaveRequestDto.StartDate,
            EndDate = leaveRequestDto.EndDate,
            Hours = leaveRequestDto.Hours,
            Comment = leaveRequestDto.Comment,
            EmployeeId = leaveRequestDto.EmployeeId,
            AbsenceReasonId = leaveRequestDto.AbsenceReasonId,
            RequestTypeId = leaveRequestDto.RequestTypeId,
        };
    }

    public static LeaveRequest MapToLeaveRequest(int id, UpdateLeaveRequestDto leaveRequestDto, int statusId)
    {
        return new LeaveRequest
        {
            Id = id,
            StartDate = leaveRequestDto.StartDate,
            EndDate = leaveRequestDto.EndDate,
            Hours = leaveRequestDto.Hours,
            Comment = leaveRequestDto.Comment,
            EmployeeId = leaveRequestDto.EmployeeId,
            AbsenceReasonId = leaveRequestDto.AbsenceReasonId,
            RequestTypeId = leaveRequestDto.RequestTypeId,
            StatusId = statusId
        };
    }

    public static LeaveRequest MapLeaveRequestStatus(int leaveRequestId, int statusId, LeaveRequest prevLeaveRequest)
    {
        return new LeaveRequest
        {
            Id = leaveRequestId,
            StartDate = prevLeaveRequest.StartDate,
            EndDate = prevLeaveRequest.EndDate,
            Hours = prevLeaveRequest.Hours,
            Comment = prevLeaveRequest.Comment,
            EmployeeId = prevLeaveRequest.EmployeeId,
            AbsenceReasonId = prevLeaveRequest.AbsenceReasonId,
            RequestTypeId = prevLeaveRequest.RequestTypeId,
            StatusId = statusId,
        };
    }

    public static ApprovalRequest MapApprovalRequestStatus(int statusId, ApprovalRequest approvalRequest)
    {
        return new ApprovalRequest
        {
            Id = approvalRequest.Id,
            Comment = approvalRequest.Comment,
            ApproverId = approvalRequest.ApproverId,
            LeaveRequestId = approvalRequest.LeaveRequestId,
            StatusId = statusId
        };
    }

    public static Employee MapEmployeeBalance(Employee employee, decimal balance)
    {
        return new Employee
        {
            Id = employee.Id,
            FullName = employee.FullName,
            OutOfOfficeBalance = balance,
            // Photo = employee.Photo,
            PeoplePartnerId = employee.PeoplePartnerId,
            PositionId = employee.PositionId,
            StatusId = employee.StatusId,
            SubdivisionId = employee.SubdivisionId,
        };
    }

    public static GetApprovalRequestDto MapToApprovalRequestDto(ApprovalRequest approvalRequest)
    {
        return new GetApprovalRequestDto
        {
            Id = approvalRequest.Id,
            Comment = approvalRequest.Comment,
            Approver = MapToEmployeeDto(approvalRequest.Approver),
            LeaveRequest = MapToLeaveRequestDto(approvalRequest.LeaveRequest),
            Status = approvalRequest.Status,
        };
    }

    public static ApprovalRequest MapToApprovalRequest(UpdateApprovalRequestDto approvalRequest, ApprovalRequest prevApprovalRequest)
    {
        return new ApprovalRequest
        {
            Id = prevApprovalRequest.Id,
            Comment = approvalRequest.Comment,
            ApproverId = prevApprovalRequest.ApproverId,
            LeaveRequestId = prevApprovalRequest.LeaveRequestId,
            StatusId = approvalRequest.StatusId,
        };
    }

    public static GetProjectDto MapToProjectDto(Project project)
    {
        return new GetProjectDto
        {
            Id = project.Id,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Comment = project.Comment,
            ProjectManager = MapToEmployeeDto(project.ProjectManager),
            ProjectType = project.ProjectType,
            Status = project.Status,
            ProjectEmployees = new List<ProjectEmployeeDto>()
        };
    }

    public static Project MapToProject(AddProjectDto project)
    {
        return new Project
        {
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            ProjectTypeId = project.ProjectTypeId,
            ProjectManagerId = project.ProjectManagerId,
            StatusId = project.StatusId,
            Comment = project.Comment
        };
    }

    public static Project MapToProject(int id, UpdateProjectDto project)
    {
        return new Project
        {
            Id = id,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            ProjectTypeId = project.ProjectTypeId,
            ProjectManagerId = project.ProjectManagerId,
            StatusId = project.StatusId,
            Comment = project.Comment
        };
    }

    public static ProjectEmployee MapToProjectEmployee(ProjectEmployeeDto projectEmployee)
    {
        return new ProjectEmployee
        {
            StartDate = projectEmployee.StartDate,
            EndDate = projectEmployee.EndDate,
            ProjectId = projectEmployee.ProjectId,
            EmployeeId = projectEmployee.EmployeeId,
        };
    }

    public static ProjectEmployeeDto MapToProjectEmployeeDto(ProjectEmployee projectEmployee)
    {
        return new ProjectEmployeeDto
        {
            StartDate = projectEmployee.StartDate,
            EndDate = projectEmployee.EndDate,
            ProjectId = projectEmployee.ProjectId,
            EmployeeId = projectEmployee.EmployeeId,
        };
    }
}
