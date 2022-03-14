namespace csi5112group1project_service.Models;

public class Admin : User
{
  public Admin(
    string id,
    string username,
    string password,
    string emailAddress
  ) : base(id, username, password, emailAddress, role: "admin")
  { }
}