using EmployeeEntity = EmployeeApi.Model.Employee.Employee;

namespace EmployeeApi.Service.Notifications;

public interface IEmployeeEmailSender
{
    Task SendAccountCreatedAsync(EmployeeEntity employee, string plainPassword);
}
