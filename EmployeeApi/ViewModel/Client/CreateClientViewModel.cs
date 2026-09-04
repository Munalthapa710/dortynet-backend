using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.ViewModel.Client
{
    public class CreateClientViewModel
    {
        [Required]
        [StringLength(100)]
        public string ClientName { get; set; } = string.Empty;
        [Required]
        [StringLength(10)]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string ProjectName { get; set; } = string.Empty;
    }
}
