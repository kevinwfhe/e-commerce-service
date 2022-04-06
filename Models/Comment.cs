namespace csi5112group1project_service.Models;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using csi5112group1project_service.Utils;
public class Comment : Post
{
  [BsonRepresentation(BsonType.ObjectId)]
  [JsonConverter(typeof(ObjectIdConverter))]
  public ObjectId parentPostId { get; set; }
  public string parentPostType { get; set; }
  public Comment(
    ObjectId id,
    string content,
    ObjectId authorId,
    ObjectId parentPostId,
    string parentPostType,
    long createTime,
    long lastUpdateTime
  ) : base(id, content, authorId, type: "comment", createTime, lastUpdateTime)
  {
    this.parentPostId = parentPostId;
    this.parentPostType = parentPostType;
  }
}

public class DetailedComment : Comment
{
  public User author { get; set; }
  public DetailedComment(
    Comment comment,
    User author
  ) : base(comment.id, comment.content, comment.authorId, comment.parentPostId, comment.parentPostType, comment.createTime, comment.lastUpdateTime)
  {
    this.author = author;
  }
}