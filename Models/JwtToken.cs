namespace csi5112group1project_service.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

public class JwtToken
{
  [JsonIgnore]
  [BsonRepresentation(BsonType.ObjectId)]
  public string id { get; set; }
  [JsonIgnore]
  public string token { get; set; }
  public JwtToken(
    string id,
    string token
  )
  {
    this.id = id;
    this.token = token;
  }
}