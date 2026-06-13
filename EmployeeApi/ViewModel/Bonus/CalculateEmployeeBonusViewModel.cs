using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.ViewModel.Bonus
{
    public class CalculateEmployeeBonusViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Employee is required.")]
        public int EmployeeId { get; set; }

        [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100.")]
        public decimal Percentage { get; set; }
    }
}