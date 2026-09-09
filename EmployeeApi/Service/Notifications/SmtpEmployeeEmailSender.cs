using EmployeeApi.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using EmployeeEntity = EmployeeApi.Model.Employee.Employee;

namespace EmployeeApi.Service.Notifications;

public class SmtpEmployeeEmailSender : IEmployeeEmailSender
{
    private readonly MailSettings _settings;

    public SmtpEmployeeEmailSender(IOptions<MailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendAccountCreatedAsync(EmployeeEntity employee, string plainPassword)
    {
        if (!_settings.Enabled)
        {
            return;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(_settings.FromEmail, _settings.FromName),
            Subject = "Your employee account has been created",
            Body = $"""
Hello {employee.Name},

Your employee account has been created.

Login Email: {employee.Email}
Password: {plainPassword}
Role: {employee.Role}

""",
            IsBodyHtml = false
        };

        message.To.Add(employee.Email);

        using var smtpClient = new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = _settings.UseSsl,
            Credentials = new NetworkCredential(_settings.UserName, _settings.Password)
        };

        await smtpClient.SendMailAsync(message);
    }
}
