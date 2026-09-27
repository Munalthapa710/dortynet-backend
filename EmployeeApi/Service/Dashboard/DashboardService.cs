using EmployeeApi.Data;
using EmployeeApi.ViewModel.Dashboard;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Service.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryViewModel> GetSummary()
    {
        var recentEmployees = await _context.Employees
            .OrderByDescending(e => e.AddedOn)
            .Take(5)
            .Select(e => new RecentActivityViewModel
            {
                Type = "Employee",
                Title = e.Name,
                CreatedOn = e.AddedOn
            })
            .ToListAsync();

        var recentTasks = await _context.AssignedTasks
            .OrderByDescending(t => t.AddedOn)
            .Take(5)
            .Select(t => new RecentActivityViewModel
            {
                Type = "Task",
                Title = t.Title,
                CreatedOn = t.AddedOn
            })
            .ToListAsync();

        return new DashboardSummaryViewModel
        {
            TotalEmployees = await _context.Employees.CountAsync(),
            ActiveEmployees = await _context.Employees.CountAsync(e => e.Status == "active"),
            InactiveEmployees = await _context.Employees.CountAsync(e => e.Status == "inactive"),
            TotalDepartments = await _context.Departments.CountAsync(),
            TotalClients = await _context.Clients.CountAsync(),
            PendingTasks = await _context.AssignedTasks.CountAsync(t => t.Status == "Pending"),
            CompletedTasks = await _context.AssignedTasks.CountAsync(t => t.Status == "Completed"),
            RecentActivities = recentEmployees
                .Concat(recentTasks)
                .OrderByDescending(a => a.CreatedOn)
                .Take(10)
                .ToList()
        };
    }
}
