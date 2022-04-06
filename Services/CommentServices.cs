namespace csi5112group1project_service.Services;
using csi5112group1project_service.Models;
using csi5112group1project_service.Utils;

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
public class CommentService
{
  private readonly IMongoCollection<Question> _questions;
  private readonly IMongoCollection<Answer> _answers;
  private readonly IMongoCollection<Comment> _comments;
  private readonly UserService _userService;
  private readonly IHttpContextAccessor _httpContextAccessor;
  public CommentService(IOptions<DatabaseSettings> databaseSettings, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, UserService userService)
  {
    var connectionString = configuration.GetValue<string>("CONNECTION_STRING");
    if (string.IsNullOrEmpty(connectionString))
    {
      // Development environment could use the connection string from the appsetting
      connectionString = databaseSettings.Value.ConnectionString;
    }
    var settings = MongoClientSettings.FromConnectionString(connectionString);
    settings.ServerApi = new ServerApi(ServerApiVersion.V1);
    var client = new MongoClient(settings);
    var database = client.GetDatabase(databaseSettings.Value.DatabaseName);
    _questions = database.GetCollection<Question>(databaseSettings.Value.QuestionCollectionName);
    _answers = database.GetCollection<Answer>(databaseSettings.Value.AnswerCollectionName);
    _comments = database.GetCollection<Comment>(databaseSettings.Value.CommentCollectionName);

    _httpContextAccessor = httpContextAccessor;
    _userService = userService;
  }

  public async Task<Comment> CreateAsync(Comment newComment)
  {
    var currentUser = (User)_httpContextAccessor.HttpContext.Items["User"];
    newComment.authorId = ObjectId.Parse(currentUser.id);
    newComment.createTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    await _comments.InsertOneAsync(newComment);

    // add a record to the comments field of the parent post;
    if (newComment.parentPostType == "question")
    {
      var updatePushOperation = Builders<Question>.Update.Push<ObjectId>(q => q.comments, newComment.id);
      await _questions.FindOneAndUpdateAsync<Question>(q => q.id == newComment.parentPostId, updatePushOperation);
    }
    else if (newComment.parentPostType == "answer")
    {
      var updateOperation = Builders<Answer>.Update.Push<ObjectId>(q => q.comments, newComment.id);
      await _answers.FindOneAndUpdateAsync<Answer>(a => a.id == newComment.parentPostId, updateOperation);
    }
    return newComment;
  }

  public async Task<DetailedComment> GetByIdAsync(string id)
  {
    ObjectId _oid = ObjectId.Parse(id);
    var comment = await _comments.Find(c => c.id == _oid).FirstOrDefaultAsync();
    if (comment == null)
    {
      return null;
    }
    var author = _userService.GetById(comment.authorId.ToString());
    var res = new DetailedComment(
      comment: comment,
      author: author
    );
    return res;
  }
  public async Task<bool> UpdateByIdAsync(string id, Comment updateComment)
  {
    bool result = false;
    ObjectId _oid = ObjectId.Parse(id);
    Comment comment = await _comments.Find(c => c.id == _oid).FirstOrDefaultAsync();
    if (comment != null)
    {
      // only the content of a comment can be updated
      comment.content = updateComment.content;
      comment.lastUpdateTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
      _comments.ReplaceOne(q => q.id == _oid, comment);
      result = true;
    }
    return result;
  }

  public async Task<bool> DeleteByIdAsync(string id)
  {
    bool result = false;
    ObjectId _oid = ObjectId.Parse(id);
    Comment comment = await _comments.Find(c => c.id == _oid).FirstOrDefaultAsync();
    if (comment != null)
    {
      // remove the record in the parent post before delete the comment
      if (comment.parentPostType == "question")
      {
        var updateOperation = Builders<Question>.Update.Pull<ObjectId>(q => q.comments, comment.id);
        await _questions.FindOneAndUpdateAsync<Question>(q => q.id == comment.parentPostId, updateOperation);
      }
      else if (comment.parentPostType == "answer")
      {
        var updateOperation = Builders<Answer>.Update.Pull<ObjectId>(q => q.comments, comment.id);
        await _answers.FindOneAndUpdateAsync<Answer>(q => q.id == comment.parentPostId, updateOperation);
      }
      _comments.DeleteOne(c => c.id == _oid);
      result = true;
    }
    return result;
  }
}