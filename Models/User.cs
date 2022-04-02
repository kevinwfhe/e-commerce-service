namespace csi5112group1project_service.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
// Base class for Client/Admin

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(Client), typeof(Admin))]
public class User
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string id { get; set; }
  public string username { get; set; }
  public string password { internal get; set; }
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