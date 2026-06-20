using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using AssignedTaskEntity = EmployeeApi.Model.AssignTask.AssignedTask;

namespace EmployeeApi.Service.AssignTask;

public class AssignTaskService : IAssignTaskService
{
    private readonly string _connString;

    public AssignTaskService(IConfiguration configuration)
    {
        _connString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string not found.");
    }

    public async Task<List<AssignedTaskEntity>> GetAll()
    {
        using var connection = new SqlConnection(_connString);
        var assignedTasks = await connection.QueryAsync<AssignedTaskEntity>(
            "[dbo].[GetAllAssignedTasks]",
            commandType: CommandType.StoredProcedure
        );

        return assignedTasks.ToList();
    }

    public async Task<List<AssignedTaskEntity>> GetByEmployeeId(int employeeId)
    {
        using var connection = new SqlConnection(_connString);
        var assignedTasks = await connection.QueryAsync<AssignedTaskEntity>(
            "[dbo].[GetAssignedTasksByEmployeeId]",
            new { EmployeeId = employeeId },
            commandType: CommandType.StoredProcedure
        );

        return assignedTasks.ToList();
    }

    public async Task<AssignedTaskEntity?> GetById(int id)
    {
        using var connection = new SqlConnection(_connString);
        return await connection.QueryFirstOrDefaultAsync<AssignedTaskEntity>(
            "[dbo].[GetAssignedTaskById]",
            new { Id = id },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<AssignedTaskEntity> Create(AssignedTaskEntity assignedTask)
    {
        using var connection = new SqlConnection(_connString);
        var createdTask = await connection.QueryFirstOrDefaultAsync<AssignedTaskEntity>(
            "[dbo].[CreateAssignedTask]",
            new
            {
                assignedTask.EmployeeId,
                assignedTask.Title,
                assignedTask.Description,
                assignedTask.DueDate,
                assignedTask.Status
            },
            commandType: CommandType.StoredProcedure
        );

        return createdTask ?? throw new KeyNotFoundException($"Employee {assignedTask.EmployeeId} was not found.");
    }

    public async Task<AssignedTaskEntity> Update(AssignedTaskEntity assignedTask)
    {
        using var connection = new SqlConnection(_connString);
        var updatedTask = await connection.QueryFirstOrDefaultAsync<AssignedTaskEntity>(
            "[dbo].[UpdateAssignedTask]",
            new
            {
                assignedTask.Id,
                assignedTask.EmployeeId,
                assignedTask.Title,
                assignedTask.Description,
                assignedTask.DueDate,
                assignedTask.Status
            },
            commandType: CommandType.StoredProcedure
        );

        return updatedTask ?? throw new KeyNotFoundException($"Assigned task {assignedTask.Id} was not found.");
    }

    public async Task<bool> Delete(int id)
    {
        using var connection = new SqlConnection(_connString);
        var affectedRows = await connection.ExecuteScalarAsync<int>(
            "[dbo].[DeleteAssignedTask]",
            new { Id = id },
            commandType: CommandType.StoredProcedure
        );

        return affectedRows > 0;
    }
}