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
  [HttpGet("{role}/{usernameOremail}/{password}", Name = "login")]
  public async Task<ActionResult<User>> login(string usernameOrEmail, string password, string role)
  {
    var user = await _AuthenticationService.login(usernameOrEmail, password, role);
    if (user is null)
    {
      _logger.LogWarning("user {usernameOrEmail} does not exist.", usernameOrEmail);
      return NotFound();
    }
    return user;
  }
  
  [HttpPost]
  public async Task<ActionResult<Client>> postAuthentication([FromBody] Client newClient)
  {
    var client = await _AuthenticationService.clientSignUp(newClient.username, newClient.emailAddress, newClient.password);
    if (client is not null)
    {
      _logger.LogWarning("user {newClient.id} already exist.", newClient.id);
      return NotFound();
    }
    return client;
  }
 
}