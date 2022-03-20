namespace csi5112group1project_service.Services;
using csi5112group1project_service.MockData;
using csi5112group1project_service.Models;
public class CategoryService
{
  public CategoryService() { }
  public async Task<Category> CreateAsync(Category newCategory)
  {
    var _id = Guid.NewGuid().ToString();
    newCategory.id = _id;
    MCategory.MockCategory.Add(newCategory);
    return newCategory;
  }
  public async Task<List<Category>> GetAsync()
  {
    return MCategory.MockCategory;
  }

  public async Task<Category> GetByIdAsync(string id)
  {
    return MCategory.MockCategory.Find(c => c.id == id);
  }

  public async Task<bool> UpdateByIdAsync(string id, Category updatedCategory)
  {
    bool result = false;
    int index = MCategory.MockCategory.FindIndex(c => c.id == id);
    if (index != -1)
    {
      MCategory.MockCategory[index] = updatedCategory;
      result = true;
    }
    return result;
  }

  public async Task<bool> DeleteByIdAsync(string id)
  {
    bool result = false;
    int index = MCategory.MockCategory.FindIndex(c => c.id == id);
    if (index != -1)
    {
      MCategory.MockCategory.RemoveAt(index);
      result = true;
    }
    return result;
  }
}