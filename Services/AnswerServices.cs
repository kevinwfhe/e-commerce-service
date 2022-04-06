namespace csi5112group1project_service.Services;
using csi5112group1project_service.Models;
using csi5112group1project_service.Utils;

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
public class AnswerService
{
  private readonly IMongoCollection<User> _users;
  private readonly IMongoCollection<Question> _questions;
  private readonly IMongoCollection<Answer> _answers;
  private readonly IMongoCollection<Comment> _comments;
  private readonly CommentService _commentService;
  private readonly UserService _userService;
  private readonly IHttpContextAccessor _httpContextAccessor;
  public AnswerService(IOptions<DatabaseSettings> databaseSettings, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, CommentService commentService, UserService userService)
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
    _users = database.GetCollection<User>(databaseSettings.Value.UserCollectionName);
    _questions = database.GetCollection<Question>(databaseSettings.Value.QuestionCollectionName);
    _answers = database.GetCollection<Answer>(databaseSettings.Value.AnswerCollectionName);
    _comments = database.GetCollection<Comment>(databaseSettings.Value.CommentCollectionName);

    _httpContextAccessor = httpContextAccessor;
    _commentService = commentService;
    _userService = userService;
  }

  public async Task<DetailedAnswer> CreateAsync(Answer newAnswer)
  {
    var currentUser = (User)_httpContextAccessor.HttpContext.Items["User"];
    newAnswer.authorId = ObjectId.Parse(currentUser.id);
    newAnswer.createTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    await _answers.InsertOneAsync(newAnswer);
    // add a record to the answers field of the parent question, and increment the answerCount of the question;
    var updatePushOperation = Builders<Question>.Update.Push<ObjectId>(q => q.answers, newAnswer.id);
    var updateCombined = Builders<Question>.Update.Combine(updatePushOperation, updatePushOperation);
    await _questions.FindOneAndUpdateAsync<Question>(q => q.id == newAnswer.parentPostId, updateCombined);
    var res = new DetailedAnswer(
      answer: newAnswer,
      comments: new List<DetailedComment>(),
      author: currentUser
    );
    return res;
  }
  public async Task<DetailedAnswer> GetByIdAsync(string id)
  {
    ObjectId _oid = ObjectId.Parse(id);
    var answer = await _answers.Find(a => a.id == _oid).FirstOrDefaultAsync();
    if (answer == null)
    {
      return null;
    }
    var comments = await Task.WhenAll(
      answer.comments
      .Select(comm => _commentService.GetByIdAsync(comm.ToString())
      .ContinueWith(comm => comm.Result))
    );

    var author = _userService.GetById(answer.authorId.ToString());
    var res = new DetailedAnswer(
      answer: answer,
      comments: comments.ToList(),
      author: author
    );
    return res;
  }
  public async Task<bool> UpdateByIdAsync(string id, Answer updateAnswer)
  {
    bool result = false;
    ObjectId _oid = ObjectId.Parse(id);
    Answer answer = await _answers.Find(a => a.id == _oid).FirstOrDefaultAsync();
    if (answer != null)
    {
      // only the content of an answer can be updated
      answer.content = updateAnswer.content;
      answer.lastUpdateTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
      _answers.ReplaceOne(a => a.id == _oid, answer);
      result = true;
    }
    return result;
  }

  public async Task<bool> DeleteByIdAsync(string id)
  {
    bool result = false;
    ObjectId _oid = ObjectId.Parse(id);
    Answer answer = await _answers.Find(a => a.id == _oid).FirstOrDefaultAsync();
    if (answer != null)
    {
      // remove the record in the parent question before deletting the answer;
      var updateOperation = Builders<Question>.Update.Pull<ObjectId>(q => q.answers, answer.id);
      await _questions.FindOneAndUpdateAsync<Question>(q => q.id == answer.parentPostId, updateOperation);

      // comments under the answer should also be deleted
      var eqFilter = Builders<Comment>.Filter.Eq(c => c.parentPostId, answer.parentPostId);
      await _comments.DeleteManyAsync(eqFilter);
      _answers.DeleteOne(o => o.id == _oid);
      result = true;
    }
    return result;
  }
}