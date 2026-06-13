using EmployeeApi.Service.Bonus;
using EmployeeApi.ViewModel.Bonus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Api.v1
{
    [ApiController]
    [Authorize(Roles = "Manager")]
    [Route("api/bonus")]
    public class BonusApiController : BaseApiController
    {
        private readonly IBonusService _service;

        public BonusApiController(IBonusService service)
        {
            _service = service;
        }

        [HttpGet("employee-dropdown")]
        public async Task<IActionResult> GetEmployeeDropdown([FromQuery] string query = "")
        {
            var employees = await _service.GetEmployeeDropdown(query);
            return SuccessResponse("Employee dropdown loaded successfully.", employees);
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate([FromBody] CalculateEmployeeBonusViewModel model)
        {
            var result = await _service.CalculateEmployeeBonus(model);

            return result is null
                ? NotFoundResponse("Employee not found.")
                : SuccessResponse("Bonus calculated successfully.", result);
        }
    }
}