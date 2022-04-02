namespace csi5112group1project_service.Services;
using csi5112group1project_service.Models;
using csi5112group1project_service.Utils;

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
public class ShippingAddressService
{
  private readonly IMongoCollection<ShippingAddress> _shippingAddress;
  private readonly IMongoCollection<Client> _clients;
  private readonly IMongoCollection<ShippingAddress> _deletedShippingAddress;
  private readonly IHttpContextAccessor _httpContextAccessor;
  public ShippingAddressService(IOptions<DatabaseSettings> databaseSettings, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
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

    _shippingAddress = database.GetCollection<ShippingAddress>(databaseSettings.Value.ShippingAddressCollectionName);
    _deletedShippingAddress = database.GetCollection<ShippingAddress>(databaseSettings.Value.DeletedShippingAddressCollectionName);
    _clients = database.GetCollection<Client>(databaseSettings.Value.UserCollectionName);

    _httpContextAccessor = httpContextAccessor;
  }
  public async Task<ShippingAddress> CreateAsync(ShippingAddress newAddress)
  {
    var currentUser = (User)_httpContextAccessor.HttpContext.Items["User"];
    newAddress.userId = currentUser.id;
    await _shippingAddress.InsertOneAsync(newAddress);

    // Add the new address's id to the shippingAddresses filed under the current user
    var identityFilter = Builders<Client>.Filter.Eq(u => u.id, currentUser.id);
    var updateOperation = Builders<Client>.Update.Push<ObjectId>(u => u.shippingAddresses, ObjectId.Parse(newAddress.id));
    await _clients.FindOneAndUpdateAsync<Client>(identityFilter, updateOperation);
    return newAddress;
  }
  public async Task<List<ShippingAddress>> GetAsync()
  {
    var currentUser = (User)_httpContextAccessor.HttpContext.Items["User"];
    var addresses = await _shippingAddress.Find(addr => addr.userId == currentUser.id).ToListAsync();
    return addresses;
  }

  public async Task<ShippingAddress> GetByIdAsync(string id)
  {
    var address = await _shippingAddress.Find(addr => addr.id == id).FirstOrDefaultAsync();
    return address;
  }

  public async Task<ShippingAddress> GetByIdFromDeletedAsync(string id)
  {
    var address = await _deletedShippingAddress.Find(addr => addr.id == id).FirstOrDefaultAsync();
    return address;
  }

  public async Task<bool> UpdateByIdAsync(string id, ShippingAddress updatedAddress)
  {
    bool result = false;
    var address = await _shippingAddress.Find(addr => addr.id == id).FirstOrDefaultAsync();
    if (address != null)
    {
      var currentUser = (User)_httpContextAccessor.HttpContext.Items["User"];
      updatedAddress.userId = currentUser.id;
      _shippingAddress.ReplaceOne(addr => addr.id == id, updatedAddress);
      result = true;
    }
    return result;
  }

  public async Task<bool> DeleteByIdAsync(string id)
  {
    bool result = false;
    var address = await _shippingAddress.Find(addr => addr.id == id).FirstOrDefaultAsync();
    if (address != null)
    {
      // soft delete
      _deletedShippingAddress.InsertOne(address);
      _shippingAddress.DeleteOne(addr => addr.id == id);
      result = true;
    }
    return result;
  }
}