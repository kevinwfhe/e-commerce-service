namespace csi5112group1project_service.Models;
using System.Text.Json.Serialization;
using MongoDB.Bson;

public class Client : User
{
  [JsonIgnore]
  public List<ObjectId> orders { get; set; }
  [JsonIgnore]
  public List<ObjectId> shippingAddresses { get; set; }
  public Client(
    string id,
    string username,
    string password,
    string emailAddress
  ) : base(id, username, password, emailAddress, role: "client")
  {
    this.orders = new List<ObjectId>();
    this.shippingAddresses = new List<ObjectId>();
  }
}