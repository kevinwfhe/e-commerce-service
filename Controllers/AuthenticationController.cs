namespace csi5112group1project_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using csi5112group1project_service.Models;
using csi5112group1project_service.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
  private readonly ILogger<AuthenticationController> _logger;
  private readonly AuthenticationService _AuthenticationService;
  public AuthenticationController(ILogger<AuthenticationController> logger, AuthenticationService AuthenticationService)
  {
    _logger = logger;
    _AuthenticationService = AuthenticationService;
  }

  // admin login: /api/Authentication/admin/{usernameOrEmail}/{password}
  // client login: /api/Authentication/client/{usernameOrEmail}/{password}
  [HttpPost("{role}", Name = "login")]
  public async Task<ActionResult<User>> login([FromBody] AuthenticationBody body, string role)
  {
    var user = await _AuthenticationService.login(body, role);
    if (user is null)
    {
      _logger.LogWarning("user {username} does not exist or password is invalid.", body.username);
      return NotFound();
    }
    return user;
  }
  
//   [HttpPost]
//   public async Task<ActionResult<User>> postAuthentication([FromBody] User newClient)
//   {
//     _logger.LogWarning("[NEWCLIENT OBJECT]", newClient);
//     var client = await _AuthenticationService.clientSignUp(newClient);
//     if (client is not null)
//     {
//       _logger.LogWarning("user {newClient.id} already exist.", newClient.id);
//       return StatusCode(403);
//     }
//     return client;
//   }

  //https://localhost:7098/api/authentication/
  [HttpPost]
  public async Task<ActionResult<Client>> postAuthentication([FromBody] Client newClient)
  {
    return newClient;

  }
 
}