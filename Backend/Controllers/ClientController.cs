using System.Reflection.Metadata.Ecma335;
using AXIOCRM.Application.Interfaces;
using AXIOCRM.Domain.Entities;
using Microsoft.AspNetCore.Mvc;


namespace ProductManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetAll()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            return client == null ? NotFound() : Ok(client);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetClientDetails(int id)
        {
            var client = await _clientService.GetClientWithOrdersAsync(id);
            return client == null ? NotFound() : Ok(client);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {

          var created =  await _clientService.CreateAsync(client);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Client client)
        {
            var cl = await _clientService.UpdateAsync(id,client);
           if (cl == null)
                return NotFound();
            return Ok(cl);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clientService.DeleteAsync(id);
            return client ? NoContent() : NotFound();
        }

       
    }
}
