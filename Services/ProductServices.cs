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
  public async Task<List<Product>> GetAsync()
  {
    return MProduct.MockProducts;
  }

  public async Task<Product> GetByIdAsync(string id)
  {
    return MProduct.MockProducts.Find(p => p.id == id);
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