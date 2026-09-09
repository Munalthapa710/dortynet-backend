using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.ViewModel.Intern
{
    public class CreateInternViewModel
    {
        [Required]
        [StringLength(100)]
        public string InternName { get; set; } = string.Empty;
        [Required]
        [StringLength(10)]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string ProjectName { get; set; } = string.Empty;
    }

}

