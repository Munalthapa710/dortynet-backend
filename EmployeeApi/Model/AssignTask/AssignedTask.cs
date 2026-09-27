using EmployeeApi.Model.Common;

namespace EmployeeApi.Model.AssignTask;

public class AssignedTask : BaseEntity
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime AssignedOn { get; set; } = DateTime.UtcNow;
}
