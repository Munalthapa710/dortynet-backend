using AssignedTaskEntity = EmployeeApi.Model.AssignTask.AssignedTask;

namespace EmployeeApi.Service.AssignTask;

public interface IAssignTaskService
{
    Task<List<AssignedTaskEntity>> GetAll();

    Task<List<AssignedTaskEntity>> GetByEmployeeId(int employeeId);

    Task<AssignedTaskEntity?> GetById(int id);

    Task<AssignedTaskEntity> Create(AssignedTaskEntity assignedTask);

    Task<AssignedTaskEntity> Update(AssignedTaskEntity assignedTask);

    Task<bool> Delete(int id);
}
