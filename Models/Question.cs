namespace csi5112group1project_service.Models;
using System.Text.Json.Serialization;
using MongoDB.Bson;
// using MongoDB.Bson.Serialization.Attributes;
public class Question : Post
{
  public string title { get; set; }

  [JsonIgnore]
  public List<ObjectId> comments { get; set; }

  [JsonIgnore]
  public List<ObjectId> answers { get; set; }

  // [BsonRepresentation(BsonType.ObjectId)]
  // string categoryId;
  public Question(
    ObjectId id,
    string title,
    string content,
    ObjectId authorId,
    long createTime,
    long lastUpdateTime
  // string categoryId
  ) : base(id, content, authorId, type: "question", createTime, lastUpdateTime)
  {
    this.title = title;
    this.comments = new List<ObjectId>();
    this.answers = new List<ObjectId>();
    // this.categoryId = categoryId;
  }
}

public class IntermediateQuestion : Question
{
  public int answerCount { get; set; }
  public int commentCount { get; set; }
  public User author { get; set; }
  public IntermediateQuestion(
    Question question,
    int answerCount,
    int commentCount,
    User author
  ) : base(question.id, question.title, question.content, question.authorId, question.createTime, question.lastUpdateTime)
  {
    this.answerCount = answerCount;
    this.commentCount = commentCount;
    this.author = author;
  }
}

public class DetailedQuestion : Question
{
  public User author { get; set; }
  public new List<DetailedComment> comments { get; set; }
  public new List<DetailedAnswer> answers { get; set; }
  public DetailedQuestion(
    Question question,
    List<DetailedComment> comments,
    List<DetailedAnswer> answers,
    User author
  ) : base(question.id, question.title, question.content, question.authorId, question.createTime, question.lastUpdateTime)
  {
    this.comments = comments;
    this.answers = answers;
    this.author = author;
  }
}