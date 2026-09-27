namespace EmployeeApi.Model.Common;

public abstract class BaseEntity
{
    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; }

    public DateTime AddedOn { get; set; } = DateTime.UtcNow;

    public int? AddedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public int? DeletedBy { get; set; }
}
