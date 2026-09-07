using EmployeeApi.Service.AssignTask;
using EmployeeApi.Service.Client;
using EmployeeApi.Service.Department;
using EmployeeApi.Service.Employee;
using EmployeeApi.Service.Notifications;

namespace EmployeeApi;

public static class EmployeeApiServiceRegistrar
{
    public static IServiceCollection AddEmployeeApiServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAssignTaskService, AssignTaskService>();

        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IEmployeeEmailSender, SmtpEmployeeEmailSender>();

        return services;
    }
}
