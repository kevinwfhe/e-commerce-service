namespace csi5112group1project_service.Models;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using csi5112group1project_service.Utils;
public class Answer : Post
{
  public List<ObjectId> comments { get; set; }

  [JsonConverter(typeof(ObjectIdConverter))]
  public ObjectId parentPostId { get; set; } // the post id of the parent question
  public Answer(
    ObjectId id,
    string content,
    ObjectId authorId,
    ObjectId parentPostId,
    long createTime,
    long lastUpdateTime
  ) : base(id, content, authorId, type: "answer", createTime, lastUpdateTime)
  {
    this.comments = new List<ObjectId>();
    this.parentPostId = parentPostId;
  }
}

public class DetailedAnswer : Answer
{
  public User author { get; set; }
  public new List<DetailedComment> comments { get; set; }

  public DetailedAnswer(
    Answer answer,
    List<DetailedComment> comments,
    User author
  ) : base(answer.id, answer.content, answer.authorId, answer.parentPostId, answer.createTime, answer.lastUpdateTime)
  {
    this.comments = comments;
    this.author = author;
  }
}