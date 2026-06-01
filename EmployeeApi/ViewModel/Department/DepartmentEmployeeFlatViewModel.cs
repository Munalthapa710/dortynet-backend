namespace EmployeeApi.ViewModel.Department
{
    public class DepartmentEmployeeFlatViewModel
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int? EmployeeId { get; set; }

        public string? EmployeeName { get; set; }

        public string? Email { get; set; }

        public string? Role { get; set; }

        public string? Status { get; set; }
    }
}