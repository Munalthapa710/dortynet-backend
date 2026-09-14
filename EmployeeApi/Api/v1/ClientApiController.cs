using EmployeeApi.Service.Client;
using EmployeeApi.ViewModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClientEntity= EmployeeApi.Model.Client.Client;

namespace EmployeeApi.Api.v1
{
    [ApiController]
   
    [Route("api/client")]
    [Authorize(Roles = "Manager,Employee")]
    public class ClientApiController : BaseApiController
    {
       private readonly IClientService _service;
        public ClientApiController(IClientService clientService)
        {
            _service = clientService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //return Ok(await _service.GetAll());
            var clients = await _service.GetAll();
            return SuccessResponse("Client list loaded successfully", clients);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _service.GetById(id);
            return client is null ? NotFoundResponse("Client not found") : SuccessResponse("Client loaded successfully", client);
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
            return CreatedResponse("Client created successfully", createdClient);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClientViewModel model)
        {
            if (await _service.GetById(id) is null)
            {
                return NotFoundResponse("Client not found");
            }
            var client = new ClientEntity
            {
                Id = id,
                ClientName = model.ClientName,
                PhoneNumber = model.PhoneNumber,
                ProjectName = model.ProjectName,
            };
            var updatedClient = await _service.Update(id, client);
            return SuccessResponse("Client updated successfully", updatedClient);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _service.GetById(id) is null)
            {
                return NotFoundResponse("Client not found");
            }
            await _service.Delete(id);
            return NoContent();
        }
    }
}
