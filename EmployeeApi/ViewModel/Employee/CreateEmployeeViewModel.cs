//use of viewmodel is to represent the data that will be sent from the client to the server when creating a new employee. It includes validation attributes to ensure that the data meets certain criteria before it is processed by the server.
//receive data , send data ,validate data , hide unnecessary data , map data to model , improve performance , enhance security , support multiple formats , facilitate testing and debugging , enable versioning , provide a clear contract between client and server , simplify data binding in UI frameworks , support localization and globalization , enable custom validation logic , reduce coupling between layers of the application , improve maintainability and readability of code.
using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.ViewModel.Employee;

public class CreateEmployeeViewModel
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

    [Range(0, double.MaxValue)]
    public decimal Salary { get; set; }

    public int? ClientId { get; set; }

    public int DepartmentId { get; set; }

    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string Role { get; set; } = "Employee";

    [RegularExpression("^(active|inactive)$", ErrorMessage = "Status must be active or inactive.")]
    public string Status { get; set; } = "active";

}
