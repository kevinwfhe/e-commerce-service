namespace csi5112group1project_service.Services;
using csi5112group1project_service.MockData;
using csi5112group1project_service.Models;
public class LoginService
{
  public AuthenticationService() { }
  public async Task<User> CreateAsync(User user)
  {
    return user;
  }

  public async Task<User> login(string usernameOremail, string password)
  {
    if (user.role == "admin") {
      return MAdmin.MockAdmins.Find(savedUser => savedUser.username == user.username && savedUser.password == user.password);
    } 
    else if (user.role == "client") {
      return MClient.MockClients.Find(savedUser => savedUser.emailAddress == user.emailAddress && savedUser.password == user.password);
    }
    return null;
  }


}