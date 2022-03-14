namespace csi5112group1project_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using csi5112group1project_service.Models;
using csi5112group1project_service.Services;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
  private readonly ILogger<ClientController> _logger;
  private readonly ClientService _clientService;
  public ClientController(ILogger<ClientController> logger, ClientService clientService)
  {
    _logger = logger;
    _clientService = clientService;
  }

  [HttpGet(Name = "GetClients")]
  public async Task<List<Client>> Get()
  {
    return await _clientService.GetAsync();
  }

  [HttpGet("{id}", Name = "GetClientById")]
  public async Task<ActionResult<Client>> GetById(string id)
  {
    var client = await _clientService.GetByIdAsync(id);
    if (client is null)
    {
      _logger.LogWarning("Client {id} could not be found.", id);
      return NotFound();
    }
    return client;
  }

  [HttpPost]
  public async Task<ActionResult> Post([FromBody] Client newClient)
  {
    await _clientService.CreateAsync(newClient);
    return CreatedAtAction(nameof(Get), new { id = newClient.id }, newClient);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult> Update(string id, Client updatedClient)
  {
    var clientToUpdate = await _clientService.GetByIdAsync(id);
    if (clientToUpdate is null)
    {
      _logger.LogWarning("Client {id} could not be found.", id);
      return NotFound();
    }
    updatedClient.id = clientToUpdate.id;
    await _clientService.UpdateByIdAsync(id, updatedClient);
    return NoContent();
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(string id)
  {
    var clientToDelete = await _clientService.GetByIdAsync(id);
    if (clientToDelete is null)
    {
      _logger.LogWarning("Client {id} could not be found.", id);
      return NotFound();
    }
    await _clientService.DeleteByIdAsync(id);
    return NoContent();
  }
}