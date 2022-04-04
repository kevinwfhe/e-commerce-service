namespace csi5112group1project_service.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MongoDB.Driver;
using csi5112group1project_service.Models;
using csi5112group1project_service.Utils;

public class JwtService
{
  private readonly string _jwtSecret;
  private readonly IMongoCollection<JwtToken> _blacklistTokens;
  private readonly IHttpContextAccessor _httpContextAccessor;
  public JwtService(IOptions<AppSettings> appSettings, IOptions<DatabaseSettings> databaseSettings, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
  {
    var connectionString = configuration.GetValue<string>("CONNECTION_STRING");
    if (string.IsNullOrEmpty(connectionString))
    {
      // Development environment could use the connection string from the appsetting
      connectionString = databaseSettings.Value.ConnectionString;
    }
    var settings = MongoClientSettings.FromConnectionString(connectionString);
    settings.ServerApi = new ServerApi(ServerApiVersion.V1);
    var client = new MongoClient(settings);
    var database = client.GetDatabase(databaseSettings.Value.DatabaseName);
    _blacklistTokens = database.GetCollection<JwtToken>(databaseSettings.Value.BlacklistTokenCollectionName);

    _jwtSecret = configuration.GetValue<string>("JWT_SECRET");
    if (string.IsNullOrEmpty(_jwtSecret))
    {
      // Development environment could use the connection string from the appsetting
      _jwtSecret = appSettings.Value.JwtSecret;
    }

    _httpContextAccessor = httpContextAccessor;
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

  public void invalidateToken()
  {
    string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
    var tokenToInvalidate = new JwtToken(id: "", token: token);
    _blacklistTokens.InsertOne(tokenToInvalidate);
  }

  public bool tokenInBlacklist(string token) {
    return _blacklistTokens.Find(t => t.token == token).Any();
  }
}