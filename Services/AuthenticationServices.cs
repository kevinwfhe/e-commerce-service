namespace csi5112group1project_service.Services;
using csi5112group1project_service.MockData;
using csi5112group1project_service.Models;
public class AuthenticationService
{
  private readonly ILogger<AuthenticationService> _logger;
  public AuthenticationService(ILogger<AuthenticationService> logger) {
    _logger = logger;
   }

  public async Task<User> login(string usernameOrEmail, string password, string role)
  {
    if (role == "admin") {
      return MAdmin.MockAdmins.Find(savedUser => savedUser.username == usernameOrEmail && savedUser.password == password);
    } 
    else if (role == "client") {
      return MClient.MockClients.Find(savedUser => savedUser.emailAddress == usernameOrEmail && savedUser.password == password);
    }
    return null;
  }

  public async Task<User> clientSignUp(User newClient) {
    var client = MClient.MockClients.Find(savedClient => newClient.username == savedClient.username && newClient.emailAddress == savedClient.emailAddress);
    _logger.LogWarning("[SAVED CLIENTS]", client.ToString());

    if (client is null) {
      var _id = Guid.NewGuid().ToString();
      return new User(_id, newClient.username, newClient.password, newClient.emailAddress, "client");
    }
    return null;
  }
}