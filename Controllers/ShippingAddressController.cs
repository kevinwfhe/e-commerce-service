namespace csi5112group1project_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using csi5112group1project_service.Models;
using csi5112group1project_service.Services;
using csi5112group1project_service.Utils;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ShippingAddressController : ControllerBase
{
  private readonly ILogger<ShippingAddressController> _logger;
  private readonly ShippingAddressService _shippingAddressService;
  public ShippingAddressController(ILogger<ShippingAddressController> logger, ShippingAddressService shoppingAddressService)
  {
    _logger = logger;
    _shippingAddressService = shoppingAddressService;
  }

  [HttpGet(Name = "GetShippingAddresses")]
  public async Task<List<ShippingAddress>> Get()
  {
    // User currentUser = (User)HttpContext.Items["User"];
    return await _shippingAddressService.GetAsync();
  }

  [HttpGet("{id}", Name = "GetAddressById")]
  public async Task<ActionResult<ShippingAddress>> GetById(string id)
  {
    var address = await _shippingAddressService.GetByIdAsync(id);
    if (address is null)
    {
      _logger.LogWarning("Address {id} could not be found.", id);
      return NotFound();
    }
    return address;
  }

  [HttpPost]
  public async Task<ActionResult> Post([FromBody] ShippingAddress newAddress)
  {
    ShippingAddress addressCreated = await _shippingAddressService.CreateAsync(newAddress);
    return CreatedAtAction(nameof(Get), addressCreated);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult> Update(string id, [FromBody] ShippingAddress updatedAddress)
  {
    var addressToUpdate = await _shippingAddressService.GetByIdAsync(id);
    if (addressToUpdate is null)
    {
      _logger.LogWarning("Address {id} could not be found.", id);
      return NotFound();
    }
    updatedAddress.id = addressToUpdate.id;
    await _shippingAddressService.UpdateByIdAsync(id, updatedAddress);
    return NoContent();
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(string id)
  {
    var addressToDelete = await _shippingAddressService.GetByIdAsync(id);
    if (addressToDelete is null)
    {
      _logger.LogWarning("Address {id} could not be found.", id);
      return NotFound();
    }
    await _shippingAddressService.DeleteByIdAsync(id);
    return NoContent();
  }
}