namespace csi5112group1project_service.Services;
using csi5112group1project_service.Models;
using csi5112group1project_service.Utils;

using Microsoft.Extensions.Options;
using MongoDB.Driver;
public class AuthenticationService
{
  private readonly ILogger<AuthenticationService> _logger;
  private readonly JwtService _JwtService;
  private readonly IMongoCollection<User> _users;
  public AuthenticationService(ILogger<AuthenticationService> logger, JwtService JwtService, IOptions<DatabaseSettings> databaseSettings)
  {
    _logger = logger;
    _JwtService = JwtService;
    var settings = MongoClientSettings.FromConnectionString(databaseSettings.Value.ConnectionString);
    settings.ServerApi = new ServerApi(ServerApiVersion.V1);
    var client = new MongoClient(settings);
    var database = client.GetDatabase(databaseSettings.Value.DatabaseName);
    _users = database.GetCollection<User>(databaseSettings.Value.UserCollectionName);
  }

  public async Task<User> Login(AuthenticationRequestBody body)
  {
    string _usernameOrEmail = body.username;
    string _password = body.password;
    var user = await _users.Find(u => u.username == _usernameOrEmail || u.emailAddress == _usernameOrEmail).FirstOrDefaultAsync();
    return user;
  }

  public AuthenticationResponseBody SignToken(User user)
  {
    string token = _JwtService.generateJwtToken(user);
    // Construct the user information that sends back to client side
    return new AuthenticationResponseBody(user, token);
  }
}