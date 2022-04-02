namespace csi5112group1project_service.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class Category
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string id { get; set; }
  public string name { get; set; }

  public Category(
    string id,
    string name
  )
  {
    this.id = id;
    this.name = name;
  }

}