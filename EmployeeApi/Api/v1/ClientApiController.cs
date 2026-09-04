using EmployeeApi.Service.Client;
using EmployeeApi.ViewModel.Client;
using ClientEntity= EmployeeApi.Model.Client.Client;

using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Api.v1
{
    [ApiController]
    [Route("api/client")]
    public class ClientApiController : ControllerBase
    {
       private readonly IClientService _service;
        public ClientApiController(IClientService clientService)
        {
            _service = clientService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _service.GetById(id);
            return client is null ? NotFound() : Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientViewModel model)
        {
            var client = new ClientEntity
            {
                ClientName = model.ClientName,
                PhoneNumber = model.PhoneNumber,
                ProjectName = model.ProjectName,
            };
            var createdClient = await _service.Create(client);
            return CreatedAtAction(nameof(GetById), new { id = createdClient.Id }, createdClient);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClientViewModel model)
        {
            if (await _service.GetById(id) is null)
            {
                return NotFound();
            }
            var client = new ClientEntity
            {
                Id = id,
                ClientName = model.ClientName,
                PhoneNumber = model.PhoneNumber,
                ProjectName = model.ProjectName,
            };
            var updatedClient = await _service.Update(id, client);
            return Ok(updatedClient);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _service.GetById(id) is null)
            {
                return NotFound();
            }
            await _service.Delete(id);
            return NoContent();
        }
    }
}
