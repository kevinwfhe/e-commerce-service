namespace csi5112group1project_service.Services;
using csi5112group1project_service.MockData;
using csi5112group1project_service.Models;
public class ProductService
{
  public ProductService() { }
  public async Task<string> CreateAsync(Product newProduct)
  {
    MProducts.MockProducts.Add(newProduct);
    return newProduct.id;
  }
  public async Task<List<Product>> GetAsync()
  {
    return MProducts.MockProducts;
  }

  public async Task<Product> GetByIdAsync(string id)
  {
    return MProducts.MockProducts.Find(p => p.id == id);
  }

  public async Task<bool> UpdateByIdAsync(string id, Product updatedProduct)
  {
    bool result = false;
    int index = MProducts.MockProducts.FindIndex(p => p.id == id);
    if (index != -1)
    {
      MProducts.MockProducts[index] = updatedProduct;
      result = true;
    }
    return result;
  }

  public async Task<bool> DeleteByIdAsync(string id)
  {
    bool result = false;
    int index = MProducts.MockProducts.FindIndex(p => p.id == id);
    if (index != -1)
    {
      MProducts.MockProducts.RemoveAt(index);
      result = true;
    }
    return result;
  }
}