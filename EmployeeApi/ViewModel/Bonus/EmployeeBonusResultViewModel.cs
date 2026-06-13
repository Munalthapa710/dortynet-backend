namespace EmployeeApi.ViewModel.Bonus
{
    public class EmployeeBonusResultViewModel
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public decimal Percentage { get; set; }

        public decimal BonusAmount { get; set; }

        public decimal TotalAmount { get; set; }
    }
}