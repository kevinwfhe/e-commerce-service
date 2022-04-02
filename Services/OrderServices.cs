namespace csi5112group1project_service.Services;
using csi5112group1project_service.Models;
using csi5112group1project_service.Utils;

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

public class OrderService
{
  private readonly IMongoCollection<Order> _orders;
  private readonly IMongoCollection<User> _users;
  private readonly IMongoCollection<Client> _clients;
  private readonly ProductService _productsService;
  private readonly ShippingAddressService _shippingAddressService;
  private readonly IHttpContextAccessor _httpContextAccessor;
  public OrderService(IOptions<DatabaseSettings> databaseSettings, IHttpContextAccessor httpContextAccessor, ShippingAddressService shippingAddressService, ProductService productService, IConfiguration configuration)
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
    _orders = database.GetCollection<Order>(databaseSettings.Value.OrderCollectionName);
    _users = database.GetCollection<User>(databaseSettings.Value.UserCollectionName);
    _clients = database.GetCollection<Client>(databaseSettings.Value.UserCollectionName);

    _httpContextAccessor = httpContextAccessor;
    _shippingAddressService = shippingAddressService;
    _productsService = productService;
  }
  public async Task<Order> CreateAsync(Order newOrder)
  {
    // attach the currentUser's id to userId of the order
    var currentUser = (User)_httpContextAccessor.HttpContext.Items["User"];
    newOrder.userId = currentUser.id;

    await _orders.InsertOneAsync(newOrder);

    // Add the new order's id to the orders filed under the current user
    var identityFilter = Builders<Client>.Filter.Eq(u => u.id, currentUser.id);
    var updateOperation = Builders<Client>.Update.Push<ObjectId>(u => u.orders, ObjectId.Parse(newOrder.id));
    await _clients.FindOneAndUpdateAsync<Client>(identityFilter, updateOperation);

    return newOrder;
  }
  public async Task<List<Order>> GetAsync()
  {
    List<Order> res;
    var currentUser = (User)_httpContextAccessor.HttpContext.Items["User"];
    if (currentUser.role == "admin")
    {
      // return all orders if the user is an admin user
      res = _orders.Find(_ => true).ToList();
    }
    else
    {
      // otherwise return only the client's orders

      // var identityFilter = Builders<Client>.Filter.Eq(u => u.id, currentUser.id);
      // // retrieve all the orderIds
      // var orderIds = _clients.Find(identityFilter)
      //   .Project(u => u.orders)
      //   .FirstOrDefault()
      //   .Select(id => id.ToString());
      // // query orders based on ids
      // var inFilter = Builders<Order>.Filter.In(o => o.id, orderIds);

      var eqFilter = Builders<Order>.Filter.Eq(o => o.userId, currentUser.id);
      res = _orders.Find(eqFilter).ToList();
    }
    return res;
  }

  public async Task<List<Order>> GetByKeywordAsync(string keyword)
  {
    var currentUser = (User)_httpContextAccessor.HttpContext.Items["User"];
    // identity filter
    var identityFilter = currentUser.role == "admin"
      ? Builders<Order>.Filter.Empty
      // admin user can access all the orders therefore has an Empty filter which matches all
      : Builders<Order>.Filter.Eq(o => o.userId, currentUser.id);

    // id of Order is ObjectId type, which can not match with regex,
    // need to append another field 'tempId', which converts ObjectId to string type
    var addFields = new BsonDocument(
      new BsonElement(
        "$addFields",
        new BsonDocument(
          new List<BsonElement>{
            new BsonElement(
              "tempId",
              new BsonDocument(
                "$toString",
                "$_id"
              )
            ),
            new BsonElement(
              "tempUserId",
              new BsonDocument(
                "$toString",
                "$userId"
              )
            )
          }
        )
      )
    );
    // use keyword as pattern to match the id string
    var orderIdRegexFilter = Builders<BsonDocument>.Filter.Regex("tempId", new BsonRegularExpression(keyword, "i"));
    // use keyword as pattern to match the userId string
    var userIdRegexFilter = Builders<BsonDocument>.Filter.Regex("tempUserId", new BsonRegularExpression(keyword, "i"));
    // either tempId or tempUserId find a match are considered valid result
    var orFilter = Builders<BsonDocument>.Filter.Or(orderIdRegexFilter, userIdRegexFilter);
    // exclude the added temp fields so that the results can fit in the Order model
    var projection = Builders<BsonDocument>.Projection.Exclude("tempId").Exclude("tempUserId");
    // perform the aggregation query
    var aggregation = _orders.Aggregate()
      .Match(identityFilter)
      .AppendStage<BsonDocument>(addFields)
      .Match(orFilter)
      .Project(projection)
      .ToList();
    // deserialize the bsondocument to Order object
    var res = new List<Order>();
    res.AddRange(aggregation.Select(x => BsonSerializer.Deserialize<Order>(x)));

    return res;
  }

  // When get an order by its id, details of the order should be retured in human-readable way
  // i.e. Detailed shipping address instead of shippingAddressId
  // i.e. Detailed products instead of productIds
  public async Task<DetailedOrder> GetByIdAsync(string id)
  {

    // Get order by id
    var order = await _orders.Find(o => o.id == id).FirstOrDefaultAsync();
    var currentUser = (User)_httpContextAccessor.HttpContext.Items["User"];
    // if is unauthorized return no results
    if (currentUser.role != "admin" && order.userId != currentUser.id)
    {
      return null;
    }

    // Get shipping address of this order by its shippingAddressId
    var shippingAddress = await _shippingAddressService.GetByIdAsync(order.shippingAddressId);
    if (shippingAddress == null)
    {
      // if the address is not found, query it from the deleted collection
      shippingAddress = await _shippingAddressService.GetByIdFromDeletedAsync(order.shippingAddressId);
    }

    // Get all the purchased products of this order using the productId within putchasedItems
    async Task<Product> getProduct(string id)
    {
      // find product first from current collection and then deleted collection
      var product = await _productsService.GetById(id);
      if (product == null)
      {
        product = await _productsService.GetByIdFromDeleted(id);
      }
      return product;
    }
    var purchasedProducts = await Task.WhenAll(
      order.purchasedItems
      .Select(item => getProduct(item.productId)
      .ContinueWith(product => new PurchasedProduct(product.Result, item.quantity)))
    );
    // consturct the DetailedOrder with all the components
    return new DetailedOrder(
      id: order.id,
      createTime: order.createTime,
      status: order.status,
      totalPrice: order.totalPrice,
      shippingAddress: shippingAddress,
      purchasedProducts: purchasedProducts.ToList()
    );
  }

  public async Task<bool> UpdateByIdAsync(string id, Order updateOrder)
  {
    bool result = false;
    Order order = await _orders.Find(o => o.id == id).FirstOrDefaultAsync();
    if (order != null)
    {
      _orders.ReplaceOne(o => o.id == id, updateOrder);
      result = true;
    }
    return result;
  }

  public async Task<bool> DeleteByIdAsync(string id)
  {
    bool result = false;
    Order order = await _orders.Find(o => o.id == id).FirstOrDefaultAsync();
    if (order != null)
    {
      _orders.DeleteOne(o => o.id == id);
      result = true;
    }
    return result;
  }


  /* -----------The following methods are for formatting the response ------------ */
  public List<Order> SortOrders(List<Order> orders, string sortIndex, string sortAsc)
  {
    if (orders.Count == 0 || sortIndex == null) return orders;
    var _sortIndex = int.Parse(sortIndex);
    var _sortAsc = int.Parse(sortAsc);
    List<Order> dataSorted;
    if (_sortIndex == 1) // sort by order status
    {
      dataSorted = _sortAsc == 1
        ? orders.OrderBy(d => d.status).ToList()
        : orders.OrderByDescending(d => d.status).ToList();
    }
    else if (_sortIndex == 2) // sort by order create time
    {
      dataSorted = _sortAsc == 1
        ? orders.OrderBy(d => d.createTime).ToList()
        : orders.OrderByDescending(d => d.createTime).ToList();
    }
    else if (_sortIndex == 3) // sort by total price
    {
      dataSorted = _sortAsc == 1
        ? orders.OrderBy(d => d.totalPrice).ToList()
        : orders.OrderByDescending(d => d.totalPrice).ToList();
    }
    else
    {
      dataSorted = orders;
    }
    return dataSorted;
  }

  public PageInfo<List<Order>> PaginateOrders(List<Order> orders, string offset, string pageSize)
  {
    if (offset == null || pageSize == null)
    {
      return new PageInfo<List<Order>>(rows: orders, totalRows: orders.Count);
    }
    var _offset = int.Parse(offset);
    var _pageSize = int.Parse(pageSize);
    int totalRows = orders.Count;
    var dataInRange = _offset + _pageSize > totalRows
      ? orders.GetRange(_offset, totalRows - _offset)
      : orders.GetRange(_offset, _pageSize);
    return new PageInfo<List<Order>>(rows: dataInRange, totalRows: totalRows);
  }
}