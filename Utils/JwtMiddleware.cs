namespace csi5112group1project_service.Utils;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using csi5112group1project_service.Services;

public class JwtMiddleware
{
  private readonly RequestDelegate _next;
  private readonly string _jwtSecret;
  private readonly UserService _userService;

  public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, UserService userService, IConfiguration configuration)
  {
    _next = next;
    _userService = userService;
    _jwtSecret = configuration.GetValue<string>("JWT_SECRET");
    if (string.IsNullOrEmpty(_jwtSecret))
    {
      // Development environment could use the connection string from the appsetting
      _jwtSecret = appSettings.Value.JwtSecret;
    }

  }

  public async Task Invoke(HttpContext context)
  {
    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
    if (token != null)
      // if token exist, process to validate token and attach user to context if token valid
      attachUserToContext(context, token);

    await _next(context);
  }

  private void attachUserToContext(HttpContext context, string token)
  {
    try
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_jwtSecret);
      tokenHandler.ValidateToken(token, new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        ClockSkew = TimeSpan.Zero
      }, out SecurityToken validatedToken);

      var jwtToken = (JwtSecurityToken)validatedToken;
      var userId = jwtToken.Claims.First(x => x.Type == "sub").Value;
      // attach user to context on successful jwt validation
      context.Items["User"] = _userService.GetById(userId);
    }
    catch
    {
      // do nothing if jwt validation fails
      // user is not attached to context so request won't have access to secure routes
    }
  }
}