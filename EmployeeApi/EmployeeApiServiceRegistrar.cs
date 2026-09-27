using EmployeeApi.Service.AssignTask;
using EmployeeApi.Service.Bonus;
using EmployeeApi.Service.Client;
using EmployeeApi.Service.Dashboard;
using EmployeeApi.Service.Department;
using EmployeeApi.Service.Employee;
using EmployeeApi.Service.Intern;
using EmployeeApi.Logging;
using EmployeeApi.Service.Files;
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
        services.AddScoped<IInternService, InternService>();
        services.AddScoped<IBonusService, BonusService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        services.AddSingleton<ILoggerSetting, DefaultLoggerSetting>();
        services.AddSingleton<IAppLogger, FileAppLogger>();
        services.AddTransient<LogErrorAttribute>();
        return services;
    }
}
