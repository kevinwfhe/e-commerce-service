namespace csi5112group1project_service.Services;
using csi5112group1project_service.MockData;
using csi5112group1project_service.Models;
public class ProductService
{
  public ProductService() { }
  public async Task<Product> CreateAsync(Product newProduct)
  {
    var _id = Guid.NewGuid().ToString();
    newProduct.id = _id;
    MProduct.MockProducts.Add(newProduct);
    return newProduct;
  }
  public async Task<PageInfo<List<Product>>> GetAsync()
  {
    var res = new PageInfo<List<Product>>(rows: MProduct.MockProducts, totalRows: MProduct.MockProducts.Count);
    return res;
  }

  public async Task<Product> GetByIdAsync(string id)
  {
    return MProduct.MockProducts.Find(p => p.id == id);
  }

  public async Task<PageInfo<List<Product>>> GetByKeywordAsync(string keyword)
  {
    var dataMatched = MProduct.MockProducts.FindAll(p => (p.title.Contains(keyword) || p.description.Contains(keyword)));
    var res = new PageInfo<List<Product>>(rows: dataMatched, totalRows: dataMatched.Count);
    return res;
  }

  public async Task<PageInfo<List<Product>>> GetByCategoryAsync(string category)
  {
    var dataInCategory = MProduct.MockProducts.FindAll(p => (p.category == category));
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
    int totalRows = MProduct.MockProducts.Count;
    var dataInCategory = _category != null
      ? MProduct.MockProducts.FindAll(p => p.category == _category)
      : MProduct.MockProducts;
    var dataMatched = _keyword != null
      ? dataInCategory.FindAll(p => (p.title.Contains(keyword) || p.description.Contains(keyword)))
      : dataInCategory;
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
    var dataInRange = _offset + _pageSize > dataSorted.Count
      ? dataSorted.GetRange(_offset, dataSorted.Count - _offset)
      : dataSorted.GetRange(_offset, _pageSize);
    var res = new PageInfo<List<Product>>(rows: dataInRange, totalRows: totalRows);
    return res;
  }
  public async Task<bool> UpdateByIdAsync(string id, Product updatedProduct)
  {
    bool result = false;
    int index = MProduct.MockProducts.FindIndex(p => p.id == id);
    if (index != -1)
    {
      MProduct.MockProducts[index] = updatedProduct;
      result = true;
    }
    return result;
  }

  public async Task<bool> DeleteByIdAsync(string id)
  {
    bool result = false;
    int index = MProduct.MockProducts.FindIndex(p => p.id == id);
    if (index != -1)
    {
      MProduct.MockProducts.RemoveAt(index);
      result = true;
    }
    return result;
  }
}