using EmployeeApi.Service.AssignTask;
using EmployeeApi.ViewModel.AssignTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AssignedTaskEntity = EmployeeApi.Model.AssignTask.AssignedTask;

namespace EmployeeApi.Api.v1;

[ApiController]
[Route("api/assign-task")] // base url for all endpoints in this controller, so all endpoints will start with api/assign-task
[Authorize(Roles = "Manager")]
public class AssignTaskApiController : BaseApiController
{
    private readonly IAssignTaskService _service;

    public AssignTaskApiController(IAssignTaskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        //return Ok(await _service.GetAll());
        var assignedTasks = await _service.GetAll();
        return SuccessResponse("Assigned task list loaded successfully", assignedTasks);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var assignedTask = await _service.GetById(id);
        return assignedTask is null ? NotFoundResponse("Assigned task not found") : SuccessResponse("Assigned task loaded successfully", assignedTask);
    }

    [HttpGet("employee/{employeeId:int}")]
    public async Task<IActionResult> GetByEmployeeId(int employeeId)
    {
        return SuccessResponse("Assigned tasks for employee loaded successfully", await _service.GetByEmployeeId(employeeId));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAssignTaskViewModel model)
    {
        try
        {
            var assignedTask = Map(model);
            var createdTask = await _service.Create(assignedTask);
            return CreatedResponse("Assigned task created successfully", createdTask);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFoundResponse(exception.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAssignTaskViewModel model)
    {
        try
        {
            var assignedTask = Map(model);
            assignedTask.Id = id;
            return SuccessResponse("Assigned task updated successfully", await _service.Update(assignedTask));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFoundResponse(exception.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _service.Delete(id) ? NoContent() : NotFoundResponse("Assigned task not found");
    }

    private static AssignedTaskEntity Map(CreateAssignTaskViewModel model)
    {
        return new AssignedTaskEntity
        {
            EmployeeId = model.EmployeeId,
            Title = model.Title,
            Description = model.Description,
            DueDate = model.DueDate,
            Status = model.Status
        };
    }
}
