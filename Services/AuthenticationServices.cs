namespace csi5112group1project_service.Services;
using csi5112group1project_service.MockData;
using csi5112group1project_service.Models;
public class AuthenticationService
{
  public AuthenticationService() { }

  public async Task<User> login(string usernameOremail, string password, string role)
  {
    if (role == "admin") {
      return MAdmin.MockAdmins.Find(savedUser => savedUser.username == usernameOremail && savedUser.password == password);
    } 
    else if (role == "client") {
      return MClient.MockClients.Find(savedUser => savedUser.emailAddress == usernameOremail && savedUser.password == password);
    }
    return null;
  }

  public async Task<Client> clientSignUp(string username, string emailAddress, string password) {
    var savedClient = MClient.MockClients.Find(savedClient => savedClient.username == username && savedClient.emailAddress == emailAddress);
    if (savedClient is null) {
      var _id = Guid.NewGuid().ToString();
      return new Client(_id, username, password, emailAddress);
    }
    else {
      return null;
    }
  }


}