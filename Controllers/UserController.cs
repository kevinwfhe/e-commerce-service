namespace csi5112group1project_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using csi5112group1project_service.Models;
using csi5112group1project_service.Services;
using csi5112group1project_service.Utils;

[ApiController]
[Route("api/")]
public class UserController : ControllerBase
{
  private readonly ILogger<UserController> _logger;
  private readonly UserService _userService;
  public UserController(ILogger<UserController> logger, UserService userService)
  {
    _logger = logger;
    _userService = userService;
  }

  [Authorize]
  [HttpGet("user", Name = "GetUserById")]
  public async Task<ActionResult<User>> GetById()
  {
    var currentUser = (User)HttpContext.Items["User"];
    var user = _userService.GetById(currentUser.id);
    if (user is null)
    {
      _logger.LogWarning("Client {id} could not be found.", currentUser.id);
      return NotFound();
    }
    return user;
  }

  [HttpPost("signup")]
  public async Task<ActionResult> Post([FromBody] User newUser)
  {
    if (newUser.role != "client")
    {
      // Only client user can be registered
      return Unauthorized();
    }
    var user = await _userService.CreateAsync(newUser);
    return CreatedAtAction(nameof(Post), user);
  }

  [Authorize]
  [HttpPut]
  public async Task<ActionResult> Update(User updatedUser)
  {
    var currentUser = (User)HttpContext.Items["User"];
    var userToUpdate = _userService.GetById(currentUser.id);
    if (userToUpdate is null)
    {
      _logger.LogWarning("Client {id} could not be found.", currentUser.id);
      return NotFound();
    }
    updatedUser.id = currentUser.id;
    await _userService.UpdateByIdAsync(currentUser.id, updatedUser);
    return NoContent();
  }
}
