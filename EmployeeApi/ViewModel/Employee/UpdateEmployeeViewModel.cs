//Having separate ViewModels makes future changes easier. and can be used to add additional properties or validation rules specific to the update operation without affecting the create operation.
using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.ViewModel.Employee;

public class UpdateEmployeeViewModel
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [StringLength(10)]
    [Phone]
    public string? PhoneNumber { get; set; }

    [StringLength(100, MinimumLength = 6)]
    public string? Password { get; set; }

    public string? Role { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Salary { get; set; }

    public int? ClientId { get; set; }

    public int DepartmentId { get; set; }

    [RegularExpression("^(active|inactive)$", ErrorMessage = "Status must be active or inactive.")]
    public string Status { get; set; } = "active";

}
