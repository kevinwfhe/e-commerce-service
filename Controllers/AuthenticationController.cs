namespace csi5112group1project_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using csi5112group1project_service.Models;
using csi5112group1project_service.Services;

[ApiController]
[Route("api/login")]
public class AuthenticationController : ControllerBase
{
  private readonly ILogger<AuthenticationController> _logger;
  private readonly AuthenticationService _AuthenticationService;
  public AuthenticationController(ILogger<AuthenticationController> logger, AuthenticationService AuthenticationService)
  {
    _logger = logger;
    _AuthenticationService = AuthenticationService;
  }

  [HttpPost(Name = "login")]
  public async Task<ActionResult<AuthenticationResponseBody>> Login([FromBody] AuthenticationRequestBody body)
  {
    var user = await _AuthenticationService.Login(body);
    if (user is null)
    {
      _logger.LogWarning("User {username} does not exist or password is invalid.", body.username);
      return NotFound();
    }
    var response =  _AuthenticationService.SignToken(user);
    return response;
  }
}