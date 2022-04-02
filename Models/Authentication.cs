namespace csi5112group1project_service.Models;

public class AuthenticationRequestBody
{
  public String username { get; set; }
  public String password { get; set; }

  public AuthenticationRequestBody(
    string username,
    string password
  )
  {
    this.username = username;
    this.password = password;
  }
}
public class AuthenticationResponseBody
{
  public User user { get; set; }
  public String jwtToken { get; set; }

  public AuthenticationResponseBody(
    User user,
    string jwtToken
  )
  {
    this.user = user;
    this.jwtToken = jwtToken;
  }
}