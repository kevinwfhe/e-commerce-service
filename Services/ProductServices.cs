namespace csi5112group1project_service.Services;
using csi5112group1project_service.Models;
using csi5112group1project_service.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
public class ProductService
{
  private readonly IMongoCollection<Product> _products;
  private readonly IMongoCollection<Product> _deletedProducts;
  private readonly AwsService _awsService;
  public ProductService(IOptions<DatabaseSettings> databaseSettings, AwsService awsService)
  {
    var settings = MongoClientSettings.FromConnectionString(databaseSettings.Value.ConnectionString);
    settings.ServerApi = new ServerApi(ServerApiVersion.V1);
    var client = new MongoClient(settings);
    var database = client.GetDatabase(databaseSettings.Value.DatabaseName);
    _products = database.GetCollection<Product>(databaseSettings.Value.ProductCollectionName);
    _deletedProducts = database.GetCollection<Product>(databaseSettings.Value.DeletedProductCollectionName);
    _awsService = awsService;
  }
  public async Task<Product> CreateAsync(Product newProduct)
  {
    var productImageS3Key = Guid.NewGuid().ToString();
    var imageUploaded = await _awsService.UploadImageAsync(newProduct.image, productImageS3Key);
    if (imageUploaded)
    {
      newProduct.image = productImageS3Key;
      await _products.InsertOneAsync(newProduct);
    }
    else
    {
      Console.WriteLine("Upload image failed.");
    }
    return newProduct;
  }
  public async Task<PageInfo<List<Product>>> GetAsync()
  {
    var products = await _products.Find(_ => true).ToListAsync();
    var res = new PageInfo<List<Product>>(rows: products, totalRows: products.Count);
    return res;
  }

  public Task<Product> GetById(string id)
  {
    return _products.Find(p => p.id == id).FirstOrDefaultAsync();
  }

  public Task<Product> GetByIdFromDeleted(string id)
  {
    return _deletedProducts.Find(p => p.id == id).FirstOrDefaultAsync();
  }

  public async Task<PageInfo<List<Product>>> GetByKeywordAsync(string keyword)
  {
    var dataMatched = await _products.Find(p => (p.title.Contains(keyword) || p.description.Contains(keyword))).ToListAsync();
    var res = new PageInfo<List<Product>>(rows: dataMatched, totalRows: dataMatched.Count);
    return res;
  }

  public async Task<PageInfo<List<Product>>> GetByCategoryAsync(string category)
  {
    var dataInCategory = await _products.Find(p => (p.category == category)).ToListAsync();
    var res = new PageInfo<List<Product>>(rows: dataInCategory, totalRows: dataInCategory.Count);
    return res;
  }

  public async Task<PageInfo<List<Product>>> GetByPageAsync(string offset, string pageSize, string sortIndex, string sortAsc, string? keyword, string? category)
  {
    var _offset = int.Parse(offset);
    var _pageSize = int.Parse(pageSize);
    var _sortIndex = int.Parse(sortIndex);
    var _sortAsc = int.Parse(sortAsc);
    var _category = category;
    var _keyword = keyword;

    // Count documents in product collection
    long totalRows = await _products.CountDocumentsAsync(new BsonDocument());

    // Filltered by category
    var dataInCategory = _category != null
      ? _products.Find(p => p.category == _category).ToList()
      : _products.Find(_ => true).ToList();

    // Filltered by keyword
    var dataMatched = _keyword != null
      ? dataInCategory.FindAll(p => (p.title.Contains(keyword) || p.description.Contains(keyword)))
      : dataInCategory;

    // Sort by index and ascendence/descendence
    List<Product> dataSorted;
    if (_sortIndex == 0) // not support yet
    {
      dataSorted = _sortAsc == 1
        ? dataMatched.OrderBy(d => d.id).ToList()
        : dataMatched.OrderByDescending(d => d.id).ToList();
    }
    else if (_sortIndex == 1)
    {
      dataSorted = _sortAsc == 1
        ? dataMatched.OrderBy(d => d.title).ToList()
        : dataMatched.OrderByDescending(d => d.title).ToList();
    }
    else if (_sortIndex == 2)
    {
      dataSorted = _sortAsc == 1
        ? dataMatched.OrderBy(d => d.category).ToList()
        : dataMatched.OrderByDescending(d => d.category).ToList();
    }
    else if (_sortIndex == 3)
    {
      dataSorted = _sortAsc == 1
        ? dataMatched.OrderBy(d => d.price).ToList()
        : dataMatched.OrderByDescending(d => d.price).ToList();
    }
    else
    {
      dataSorted = dataMatched;
    }

    // Paginated data
    var dataInRange = _offset + _pageSize > dataSorted.Count
      ? dataSorted.GetRange(_offset, dataSorted.Count - _offset)
      : dataSorted.GetRange(_offset, _pageSize);

    var res = new PageInfo<List<Product>>(rows: dataInRange, totalRows: totalRows);
    return res;
  }
  public async Task<bool> UpdateByIdAsync(string id, Product updatedProduct)
  {
    bool result = false;
    Product product = await _products.Find(p => p.id == id).FirstOrDefaultAsync();
    if (product != null)
    {
      _products.ReplaceOne(p => p.id == id, updatedProduct);
      result = true;
    }
    return result;
  }

  public async Task<bool> DeleteByIdAsync(string id)
  {
    bool result = false;
    Product product = await _products.Find(p => p.id == id).FirstOrDefaultAsync();
    if (product != null)
    {
      // soft delete
      _deletedProducts.InsertOne(product);
      _products.DeleteOne(p => p.id == id);
      result = true;
    }
    return result;
  }
}