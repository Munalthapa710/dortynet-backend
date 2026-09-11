namespace EmployeeApi.ViewModel.Employee
{
    public class EmployeeListViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public decimal Salary { get; set; }

        public int DepartmentId { get; set; }

        public string? DepartmentName { get; set; }

        public int? ClientId { get; set; }

        public string? ClientName { get; set; }

        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public int RowTotal { get; set; }
    }
}
