using EmployeeApi.ViewModel.Dashboard;

namespace EmployeeApi.Service.Dashboard;

public interface IDashboardService
{
    Task<DashboardSummaryViewModel> GetSummary();
}
