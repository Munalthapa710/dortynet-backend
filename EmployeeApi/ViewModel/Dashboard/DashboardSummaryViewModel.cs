namespace EmployeeApi.ViewModel.Dashboard;

public class DashboardSummaryViewModel
{
    public int TotalEmployees { get; set; }

    public int ActiveEmployees { get; set; }

    public int InactiveEmployees { get; set; }

    public int TotalDepartments { get; set; }

    public int TotalClients { get; set; }

    public int PendingTasks { get; set; }

    public int CompletedTasks { get; set; }

    public List<RecentActivityViewModel> RecentActivities { get; set; } = new();
}
