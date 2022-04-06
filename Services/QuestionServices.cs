namespace csi5112group1project_service.Services;
using csi5112group1project_service.Models;
using csi5112group1project_service.Utils;

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
public class QuestionService
{
  private readonly IMongoCollection<Question> _questions;
  private readonly IMongoCollection<Answer> _answer;
  private readonly IMongoCollection<Comment> _comments;
  private readonly AnswerService _answerService;
  private readonly CommentService _commentService;
  private readonly UserService _userService;
  private readonly IHttpContextAccessor _httpContextAccessor;
  public QuestionService(IOptions<DatabaseSettings> databaseSettings, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, AnswerService answerService, UserService userService, CommentService commentService)
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
    _answer = database.GetCollection<Answer>(databaseSettings.Value.AnswerCollectionName);
    _comments = database.GetCollection<Comment>(databaseSettings.Value.CommentCollectionName);

    _httpContextAccessor = httpContextAccessor;
    _answerService = answerService;
    _userService = userService;
    _commentService = commentService;
  }

  public async Task<Question> CreateAsync(Question newQuestion)
  {
    // set the currentUser's id as the authorId of the question
    var currentUser = (User)_httpContextAccessor.HttpContext.Items["User"];
    newQuestion.authorId = ObjectId.Parse(currentUser.id);
    newQuestion.createTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    await _questions.InsertOneAsync(newQuestion);
    return newQuestion;
  }

  public async Task<List<IntermediateQuestion>> GetAsync()
  {
    var question = await _questions.Find(_ => true).ToListAsync();
    IntermediateQuestion questionToImQuestion(Question q)
    {
      var author = _userService.GetById(q.authorId.ToString());
      return new IntermediateQuestion(q, q.answers.Count(), q.comments.Count(), author);
    }
    var res = question.Select((q) => questionToImQuestion(q)).ToList();
    return res;
  }

  public async Task<DetailedQuestion> GetByIdAsync(string id)
  {
    ObjectId _oid = ObjectId.Parse(id);
    var question = await _questions.Find(q => q.id == _oid).FirstOrDefaultAsync();
    if (question == null)
    {
      return null;
    }
    // retrieve comments under the question
    var comments = await Task.WhenAll(
      question.comments
      .Select(comm => _commentService.GetByIdAsync(comm.ToString())
      .ContinueWith(answer => answer.Result))
    );
    // retrieve all answers under the question
    var answers = await Task.WhenAll(
      question.answers
      .Select(ans => _answerService.GetByIdAsync(ans.ToString())
      .ContinueWith(answer => answer.Result))
    );
    var author = _userService.GetById(question.authorId.ToString());
    var res = new DetailedQuestion(
      question: question,
      comments: comments.ToList(),
      answers: answers.ToList(),
      author
    );
    return res;
  }
  public async Task<bool> UpdateByIdAsync(string id, Question updateQuestion)
  {
    bool result = false;
    ObjectId _oid = ObjectId.Parse(id);
    Question question = await _questions.Find(q => q.id == _oid).FirstOrDefaultAsync();
    if (question != null)
    {
      // only content fields can be updated
      question.content = updateQuestion.content;
      question.lastUpdateTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
      _questions.ReplaceOne(q => q.id == _oid, question);
      result = true;
    }
    return result;
  }

  public async Task<bool> DeleteByIdAsync(string id)
  {
    bool result = false;
    ObjectId _oid = ObjectId.Parse(id);
    Question question = await _questions.Find(q => q.id == _oid).FirstOrDefaultAsync();
    if (question != null)
    {
      // comments under the question should be deleted
      var commentEqFilter = Builders<Comment>.Filter.Eq(c => c.parentPostId, question.id);

      // comments under the answers of the question should also be deleted
      var answersUnderQuestion = question.answers;
      var commentInFilter = Builders<Comment>.Filter.In(c => c.parentPostId, answersUnderQuestion);
      // combine filters
      var orFilter = Builders<Comment>.Filter.Or(commentEqFilter, commentInFilter);
      await _comments.DeleteManyAsync(orFilter);

      // answers under the question should be deleted
      var answerEqFilter = Builders<Answer>.Filter.Eq(a => a.parentPostId, question.id);
      await _answer.DeleteManyAsync(answerEqFilter);

      // finally delete the question itself
      await _questions.DeleteOneAsync(o => o.id == _oid);
      result = true;
    }
    return result;
  }
}