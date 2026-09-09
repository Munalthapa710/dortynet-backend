using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.ViewModel.Intern
{
    public class CreateInternViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Description { get; set; } = string.Empty;
    }

}

