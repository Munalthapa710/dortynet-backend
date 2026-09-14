using EmployeeApi.Service.Intern;
using EmployeeApi.ViewModel.Intern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InternEntity = EmployeeApi.Model.Intern.Intern;

namespace EmployeeApi.Api.v1
{
    [ApiController]
    [Authorize(Roles = "Manager,Employee")]
    [Route("api/intern")]
    public class InternApiController : BaseApiController
    {
        private readonly IInternService _service;

        public InternApiController(IInternService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var interns = await _service.GetAll();
            return SuccessResponse("Intern list loaded successfully", interns);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var intern = await _service.GetById(id);

            return intern is null
                ? NotFoundResponse("Intern not found")
                : SuccessResponse("Intern loaded successfully", intern);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInternViewModel model)
        {
            var intern = new InternEntity
            {
                Name = model.Name,
                Description = model.Description
            };

            var createdIntern = await _service.Create(intern);

            //return CreatedAtAction(
            //    nameof(GetById),
            //    new { id = createdIntern.Id },
            //    createdIntern);
            return CreatedResponse("Intern created successfully", createdIntern);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInternViewModel model)
        {
            if (await _service.GetById(id) is null)
            {
                return NotFoundResponse("Intern not found");
            }

            var intern = new InternEntity
            {
                Id = id,
                Name = model.Name,
                Description = model.Description
            };

            return SuccessResponse("Intern updated successfully", await _service.Update(id, intern));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _service.Delete(id)
                ? NotFoundResponse("Intern deleted successfully")
                : NotFoundResponse("Intern not found");
        }

        [HttpGet("dropdown")]
        public async Task<IActionResult> GetDropdown([FromQuery] string query = "")
        {
            var interns = await _service.GetDropdown(query);
            return SuccessResponse("Intern dropdown loaded successfully.", interns);
        }
    }
}
