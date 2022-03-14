namespace csi5112group1project_service.Models;

// Base class for Client/Admin
public class User
{
  public string id { get; set; }
  public string username { get; set; }
  public string password { get; set; }
  public string emailAddress { get; set; }
  public string role { get; set; }
  public User(
    string id,
    string username,
    string password,
    string emailAddress,
    string role
  )
  {
    this.id = id;
    this.username = username;
    this.password = password;
    this.emailAddress = emailAddress;
    this.role = role;
  }
}