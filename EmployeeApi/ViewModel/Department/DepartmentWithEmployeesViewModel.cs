namespace EmployeeApi.ViewModel.Department
{
    public class DepartmentWithEmployeesViewModel
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public List<DepartmentEmployeeViewModel> Employees { get; set; } = new();
    }
}