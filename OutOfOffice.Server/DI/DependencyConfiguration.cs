using OutOfOffice.Interfaces.Repositories;
using OutOfOffice.Interfaces.Services;
using OutOfOffice.Repositories;
using OutOfOffice.Services;

namespace OutOfOffice.Server.DI;

public static class DependencyConfiguration
{
    public static IServiceCollection ConfigureDependency(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
        services.AddScoped<IApprovalRequestRepository, ApprovalRequestRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();

        // Services
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ILeaveRequestService, LeaveRequestService>();
        services.AddScoped<IApprovalRequestService, ApprovalRequestService>();
        services.AddScoped<IProjectService, ProjectService>();

        return services;
    }
}
