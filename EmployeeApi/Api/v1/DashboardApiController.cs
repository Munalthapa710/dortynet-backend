using EmployeeApi.Service.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Api.v1;

[ApiController]
[Authorize(Roles = "Manager,Employee")]
[Route("api/dashboard")]
public class DashboardApiController : BaseApiController
{
    private readonly IDashboardService _service;

    public DashboardApiController(IDashboardService service)
    {
        _service = service;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var summary = await _service.GetSummary();
        return SuccessResponse("Dashboard summary loaded successfully.", summary);
    }
}
