using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.ViewModel.AssignTask;

public class CreateAssignTaskViewModel
{
    [Range(1, int.MaxValue)]
    public int EmployeeId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Pending";
}
