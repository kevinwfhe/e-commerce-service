namespace csi5112group1project_service.Services;
using csi5112group1project_service.Models;
using csi5112group1project_service.Utils;

using Microsoft.Extensions.Options;
using MongoDB.Driver;
public class CategoryService
{
  private readonly IMongoCollection<Category> _categories;
  private readonly IMongoCollection<Category> _deletedCategories;
  public CategoryService(IOptions<DatabaseSettings> databaseSettings, IConfiguration configuration)
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
    _categories = database.GetCollection<Category>(databaseSettings.Value.CategoryCollectionName);
    _deletedCategories = database.GetCollection<Category>(databaseSettings.Value.DeletedCategoryCollectionName);
  }
  public async Task<Category> CreateAsync(Category newCategory)
  {
    bool nameExist = _categories.Find(c => c.name == newCategory.name).Any();
    if (nameExist)
    {
      throw new Exception("category exist");
    }
    await _categories.InsertOneAsync(newCategory);
    return newCategory;
  }
  public async Task<List<Category>> GetAsync()
  {
    return await _categories.Find(_ => true).ToListAsync();
  }

  public async Task<Category> GetByIdAsync(string id)
  {
    return await _categories.Find(c => c.id == id).FirstOrDefaultAsync();
  }

  public async Task<Category> GetByIdFromDeletedAsync(string id)
  {
    return await _deletedCategories.Find(c => c.id == id).FirstOrDefaultAsync();
  }

  public async Task<bool> UpdateByIdAsync(string id, Category updatedCategory)
  {
    bool nameExist = _categories.Find(c => c.name == updatedCategory.name).Any();
    if (nameExist)
    {
      throw new Exception("category exist");
    }
    bool result = false;
    var category = await _categories.Find(c => c.id == id).FirstOrDefaultAsync();
    if (category != null)
    {
      var res = _categories.ReplaceOne(c => c.id == id, updatedCategory);
      result = res.IsAcknowledged;
    }
    return result;
  }

  public async Task<bool> DeleteByIdAsync(string id)
  {
    bool result = false;
    var category = await _categories.Find(c => c.id == id).FirstOrDefaultAsync();
    if (category != null)
    {
      // soft delete
      _deletedCategories.InsertOne(category);
      var res = _categories.DeleteOne(c => c.id == id);
      result = res.IsAcknowledged;
    }
    return result;
  }
}