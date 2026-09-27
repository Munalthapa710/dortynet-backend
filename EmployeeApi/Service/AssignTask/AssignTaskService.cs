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
            .OrderByDescending(t => t.AssignedOn)
            .ToListAsync();
    }

    public async Task<List<AssignedTaskEntity>> GetByEmployeeId(int employeeId)
    {
        return await _context.AssignedTasks
            .Where(t => t.EmployeeId == employeeId)
            .OrderByDescending(t => t.AssignedOn)
            .ToListAsync();
    }

    public async Task<AssignedTaskEntity?> GetById(int id)
    {
        return await _context.AssignedTasks.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<AssignedTaskEntity> Create(AssignedTaskEntity assignedTask)
    {
        var employeeExists = await _context.Employees.AnyAsync(e => e.Id == assignedTask.EmployeeId);
        if (!employeeExists)
        {
            throw new KeyNotFoundException($"Employee {assignedTask.EmployeeId} was not found.");
        }

        _context.AssignedTasks.Add(assignedTask);
        await _context.SaveChangesAsync();
        return assignedTask;
    }

    public async Task<AssignedTaskEntity> Update(AssignedTaskEntity assignedTask)
    {
        var existingTask = await _context.AssignedTasks.FirstOrDefaultAsync(t => t.Id == assignedTask.Id)
            ?? throw new KeyNotFoundException($"Assigned task {assignedTask.Id} was not found.");

        var employeeExists = await _context.Employees.AnyAsync(e => e.Id == assignedTask.EmployeeId);
        if (!employeeExists)
        {
            throw new KeyNotFoundException($"Employee {assignedTask.EmployeeId} was not found.");
        }

        existingTask.EmployeeId = assignedTask.EmployeeId;
        existingTask.Title = assignedTask.Title;
        existingTask.Description = assignedTask.Description;
        existingTask.DueDate = assignedTask.DueDate;
        existingTask.Status = assignedTask.Status;

        await _context.SaveChangesAsync();
        return existingTask;
    }

    public async Task<bool> Delete(int id)
    {
        var assignedTask = await _context.AssignedTasks.FirstOrDefaultAsync(t => t.Id == id);
        if (assignedTask is null)
        {
            return false;
        }

        assignedTask.IsDeleted = true;
        assignedTask.IsActive = false;
        assignedTask.DeletedOn = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}