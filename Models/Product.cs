namespace csi5112group1project_service.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class Product
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string id { get; set; }
  public string image { get; set; }
  public string title { get; set; }
  public double price { get; set; }
  public string description { get; set; }
  
  [BsonRepresentation(BsonType.ObjectId)]
  public string category { get; set; }

  public double? size { get; set; }

  public Product(
    string id,
    string image,
    string title,
    double price,
    string description,
    string category,
    double? size = null
  )
  {
    this.id = id;
    this.image = image;
    this.title = title;
    this.price = price;
    this.description = description;
    this.category = category;
    this.size = size;
  }
}
