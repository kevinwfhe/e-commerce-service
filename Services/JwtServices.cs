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
  private readonly AppSettings _appSettings;
  public JwtService(IOptions<AppSettings> appSettings)
  {
    _appSettings = appSettings.Value;
  }

  public string generateJwtToken(User user)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);
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