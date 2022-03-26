namespace csi5112group1project_service.Services;
using csi5112group1project_service.MockData;
using csi5112group1project_service.Models;
public class AuthenticationService
{
  private readonly ILogger<AuthenticationService> _logger;
  public AuthenticationService(ILogger<AuthenticationService> logger) {
    _logger = logger;
   }

  public async Task<User> login(AuthenticationBody body, string role)
  {
    string _usernameOrEmail = body.username;
    string _password = body.password;
    if (role == "admin")
    {
      return MAdmin.MockAdmins.Find(savedUser => (savedUser.username == _usernameOrEmail) || (savedUser.emailAddress == _usernameOrEmail) && savedUser.password == _password);
    }
    else if (role == "client")
    {
      return MClient.MockClients.Find(savedUser => (savedUser.username == _usernameOrEmail) || (savedUser.emailAddress == _usernameOrEmail) && savedUser.password == _password);
    }
    return null;
  }

  public async Task<Client> clientSignUp(string username, string emailAddress, string password)
  {
    var savedClient = MClient.MockClients.Find(savedClient => savedClient.username == username && savedClient.emailAddress == emailAddress);
    if (savedClient is null)
    {
      var _id = Guid.NewGuid().ToString();
      return new Client(_id, username, password, emailAddress);
    }
    else
    {
      return null;
    }
    return null;
  }
}