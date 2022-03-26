namespace csi5112group1project_service.Models;

public class AuthenticationBody
{
  public String username { get; set; }
  public String password { get; set; }

  public AuthenticationBody(
    string username,
    string password
  )
  {
    this.username = username;
    this.password = password;
  }
}