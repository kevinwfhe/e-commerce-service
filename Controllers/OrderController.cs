namespace csi5112group1project_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using csi5112group1project_service.Models;
using csi5112group1project_service.Services;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
  private readonly ILogger<OrderController> _logger;
  private readonly OrderService _orderService;
  public OrderController(ILogger<OrderController> logger, OrderService orderService)
  {
    _logger = logger;
    _orderService = orderService;
  }

  [HttpGet(Name = "GetOrders")]
  public async Task<List<Order>> Get()
  {
    return await _orderService.GetAsync();
  }

  [HttpGet("{id}", Name = "GetOrderById")]
  public async Task<ActionResult<DetailedOrder>> GetById(string id)
  {
    var detaileOrder = await _orderService.GetByIdAsync(id);
    if (detaileOrder is null)
    {
      _logger.LogWarning("Order {id} could not be found.", id);
      return NotFound();
    }
    return detaileOrder;
  }

  [HttpPost]
  public async Task<ActionResult> Post([FromBody] Order newOrder)
  {
    Order orderCreated = await _orderService.CreateAsync(newOrder);
    return CreatedAtAction(nameof(Get), orderCreated);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult> Update(string id, Order updatedOrder)
  {
    var orderToUpdate = await _orderService.GetByIdAsync(id);
    if (orderToUpdate is null)
    {
      _logger.LogWarning("Order {id} could not be found.", id);
      return NotFound();
    }
    updatedOrder.id = orderToUpdate.id;
    await _orderService.UpdateByIdAsync(id, updatedOrder);
    return NoContent();
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(string id)
  {
    var orderToDelete = await _orderService.GetByIdAsync(id);
    if (orderToDelete is null)
    {
      _logger.LogWarning("Order {id} could not be found.", id);
      return NotFound();
    }
    await _orderService.DeleteByIdAsync(id);
    return NoContent();
  }
}