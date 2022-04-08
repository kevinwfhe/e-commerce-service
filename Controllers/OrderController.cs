namespace csi5112group1project_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using csi5112group1project_service.Models;
using csi5112group1project_service.Services;
using csi5112group1project_service.Utils;

[Authorize]
[ApiController]
[Route("api/")]
public class OrderController : ControllerBase
{
  private readonly ILogger<OrderController> _logger;
  private readonly OrderService _orderService;
  public OrderController(ILogger<OrderController> logger, OrderService orderService)
  {
    _logger = logger;
    _orderService = orderService;
  }

  [HttpGet("Order", Name = "GetOrders")]
  public async Task<PageInfo<List<Order>>> Get([FromQuery] string? keyword, [FromQuery] string? offset, [FromQuery] string? pageSize, [FromQuery] string? sortIndex, [FromQuery] string? sortAsc)
  {
    List<Order> orders;
    orders = keyword != null
      ? await _orderService.GetByKeywordAsync(keyword)
      : await _orderService.GetAsync();
    orders = _orderService.SortOrders(orders, sortIndex, sortAsc);
    var pagedOrders = _orderService.PaginateOrders(orders, offset, pageSize);
    return pagedOrders;
  }

  [HttpGet("Order/{id}", Name = "GetOrderById")]
  public async Task<ActionResult<DetailedOrder>> GetById(string id)
  {
    var detaileOrder = await _orderService.GetByIdAsync(id);
    if (detaileOrder is null)
    {
      // noted that when the query result is null, it is possible because of unauthorization
      // in that case, we dont want the sender aware of the existence of this order,
      // hence return 404 if the order is null
      _logger.LogWarning("Order {id} could not be found, or the request sender has no access to it.", id);
      return NotFound();
    }
    return detaileOrder;
  }

  [HttpPost("Order")]
  public async Task<ActionResult> Post([FromBody] Order newOrder)
  {
    Order orderCreated = await _orderService.CreateAsync(newOrder);
    return CreatedAtAction(nameof(Get), orderCreated);
  }

  [HttpPut("Order/{id}")]
  public async Task<ActionResult> Update(string id, [FromBody] Order updatedOrder)
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

  [HttpDelete("Order/{id}")]
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

  [HttpGet("SendOrder/{id}")]
  public async Task<ActionResult> Email(string id)
  {
    var result = await _orderService.SendOrderToEmail(id);
    if (result == "user not found")
    {
      return BadRequest("User not found");
    }
    return Ok(result);
  }
}