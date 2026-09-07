using EmployeeApi.Service.Department;
using EmployeeApi.ViewModel.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DepartmentEntity = EmployeeApi.Model.Department.Department;

namespace EmployeeApi.Api.v1
{
    [ApiController]
    [Authorize(Roles = "Manager,Employee")]
    [Route("api/department")]
    public class DepartmentApiController : ControllerBase
    {
        private readonly IDepartmentService _service;

        public DepartmentApiController(
            IDepartmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department =
                await _service.GetById(id);

            return department is null
                ? NotFound()
                : Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateDepartmentViewModel model)
        {
            var department = new DepartmentEntity
            {
                Name = model.Name,
                Description = model.Description
            };

            var createdDepartment =
                await _service.Create(department);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdDepartment.Id },
                createdDepartment);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateDepartmentViewModel model)
        {
            if (await _service.GetById(id) is null)
            {
                return NotFound();
            }

            var department = new DepartmentEntity
            {
                Id = id,
                Name = model.Name,
                Description = model.Description
            };

            return Ok(
                await _service.Update(department));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _service.Delete(id)
                ? NoContent()
                : NotFound();
        }
    }
}