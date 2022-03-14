namespace csi5112group1project_service.Services;
using csi5112group1project_service.MockData;
using csi5112group1project_service.Models;
public class ProductService
{
  public ProductService() { }
  public async Task<string> CreateAsync(Product newProduct)
  {
    MProduct.MockProducts.Add(newProduct);
    return newProduct.id;
  }
  public async Task<List<Product>> GetAsync()
  {
    Console.WriteLine(MProduct.MockProducts);
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