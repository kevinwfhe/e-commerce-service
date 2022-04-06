namespace csi5112group1project_service.Models;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using csi5112group1project_service.Utils;

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(Question), typeof(Answer), typeof(Comment))]
public class Post
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  [JsonConverter(typeof(ObjectIdConverter))]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
  public ObjectId id { get; set; }
  public string content { get; set; }
  public int helpfulCount { get; set; }

  [BsonRepresentation(BsonType.ObjectId)]
  [JsonIgnore]
  public ObjectId authorId { get; set; }

  [JsonIgnore]
  public string type { get; set; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
  public long createTime { get; set; } // Unix timestamp
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
  public long lastUpdateTime { get; set; } // Unix timestamp

  public Post(
    ObjectId id,
    string content,
    ObjectId authorId,
    string type,
    long createTime,
    long lastUpdateTime
  )
  {
    this.id = id;
    this.content = content;
    this.helpfulCount = 0;
    this.authorId = authorId;
    this.type = type;
    this.createTime = createTime;
    this.lastUpdateTime = lastUpdateTime;
  }

}