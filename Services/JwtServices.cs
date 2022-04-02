namespace csi5112group1project_service.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using csi5112group1project_service.Models;
using csi5112group1project_service.Utils;

public class JwtService
{
  private readonly string _jwtSecret;
  public JwtService(IOptions<AppSettings> appSettings, IConfiguration configuration)
  {
    _jwtSecret = configuration.GetValue<string>("JWT_SECRET");
    if (string.IsNullOrEmpty(_jwtSecret))
    {
      // Development environment could use the connection string from the appsetting
      _jwtSecret = appSettings.Value.JwtSecret;
    }
  }

  public string generateJwtToken(User user)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_jwtSecret);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new[] { new Claim("sub", user.id.ToString()) }),
      // generate token that is valid for 2 hours
      Expires = DateTime.UtcNow.AddHours(2),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
}