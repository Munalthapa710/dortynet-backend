using EmployeeApi.Data;
using Microsoft.EntityFrameworkCore;
using AssignedTaskEntity = EmployeeApi.Model.AssignTask.AssignedTask;

namespace EmployeeApi.Service.AssignTask;

public class AssignTaskService : IAssignTaskService
{
    private readonly ApplicationDbContext _context;

    public AssignTaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AssignedTaskEntity>> GetAll()
    {
        return await _context.AssignedTasks
            .OrderByDescending(task => task.AssignedOn)
            .ToListAsync();
    }

    public async Task<List<AssignedTaskEntity>> GetByEmployeeId(int employeeId)
    {
        return await _context.AssignedTasks
            .Where(task => task.EmployeeId == employeeId)
            .OrderByDescending(task => task.AssignedOn)
            .ToListAsync();
    }

    public async Task<AssignedTaskEntity?> GetById(int id)
    {
        return await _context.AssignedTasks.FindAsync(id);
    }

    public async Task<AssignedTaskEntity> Create(AssignedTaskEntity assignedTask)
    {
        await EnsureEmployeeExists(assignedTask.EmployeeId);

        _context.AssignedTasks.Add(assignedTask);
        await _context.SaveChangesAsync();
        return assignedTask;
    }

    public async Task<AssignedTaskEntity> Update(AssignedTaskEntity assignedTask)
    {
        await EnsureEmployeeExists(assignedTask.EmployeeId);

        var existingTask = await _context.AssignedTasks.FindAsync(assignedTask.Id)
            ?? throw new KeyNotFoundException($"Assigned task {assignedTask.Id} was not found.");

        assignedTask.AssignedOn = existingTask.AssignedOn;
        _context.Entry(existingTask).CurrentValues.SetValues(assignedTask);
        await _context.SaveChangesAsync();
        return existingTask;
    }

    public async Task<bool> Delete(int id)
    {
        var assignedTask = await _context.AssignedTasks.FindAsync(id);

        if (assignedTask is null)
        {
            return false;
        }

        _context.AssignedTasks.Remove(assignedTask);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task EnsureEmployeeExists(int employeeId)
    {
        if (!await _context.Employees.AnyAsync(employee => employee.Id == employeeId))
        {
            throw new KeyNotFoundException($"Employee {employeeId} was not found.");
        }
    }
}
