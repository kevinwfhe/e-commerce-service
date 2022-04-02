namespace csi5112group1project_service.Services;
using csi5112group1project_service.Models;
using csi5112group1project_service.Utils;

using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class UserService
{
  private readonly IMongoCollection<User> _users;
  public UserService(IOptions<DatabaseSettings> databaseSettings, IConfiguration configuration)
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
  }

  public async Task<User> CreateAsync(User newUser)
  {
    if (newUser.role == "admin")
    {
      var newAdmin = new Admin(id: "", newUser.username, newUser.password, newUser.emailAddress);
      await _users.InsertOneAsync(newAdmin);
      return newAdmin;
    }
    else
    {
      var newClient = new Client(id: "", newUser.username, newUser.password, newUser.emailAddress);
      await _users.InsertOneAsync(newClient);
      return newClient;
    }
  }
  public User GetById(string id)
  {
    var user = _users.Find<User>(user => user.id == id).FirstOrDefault();
    return user;
  }
  public async Task<bool> UpdateByIdAsync(string id, User updateUser)
  {
    bool result = false;
    User user = await _users.Find(u => u.id == id).FirstOrDefaultAsync();
    if (user != null)
    {
      _users.ReplaceOne(o => o.id == id, updateUser);
      result = true;
    }
    return result;
  }
}