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

  [HttpGet("{usernameOremail}/{password}", Name = "login")]
  public async Task<ActionResult<User>> login(string usernameOremail, string password)
  {
    var user = await _AuthenticationService.login(usernameOremail, password);
    if (user is null)
    {
      _logger.LogWarning("user {usernameOremail} does not exist.", usernameOremail);
      return NotFound();
    }
    return user;
  }
 
}