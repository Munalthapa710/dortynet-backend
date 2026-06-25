using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.ViewModel.Department
{
    public class CreateDepartmentViewModel
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}