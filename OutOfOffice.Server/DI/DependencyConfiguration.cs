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

        // Services
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ILeaveRequestService, LeaveRequestService>();

        return services;
    }
}
