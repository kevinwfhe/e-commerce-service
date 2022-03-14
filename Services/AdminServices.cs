namespace csi5112group1project_service.Services;
using csi5112group1project_service.MockData;
using csi5112group1project_service.Models;
public class AdminService
{
  public AdminService() { }
  public async Task<string> CreateAsync(Admin newAdmin)
  {
    MAdmin.MockAdmins.Add(newAdmin);
    return newAdmin.id;
  }
  public async Task<List<Admin>> GetAsync()
  {
    Console.WriteLine(MAdmin.MockAdmins);
    return MAdmin.MockAdmins;
  }

  public async Task<Admin> GetByIdAsync(string id)
  {
    return MAdmin.MockAdmins.Find(a => a.id == id);
  }

  public async Task<bool> UpdateByIdAsync(string id, Admin updatedAdmin)
  {
    bool result = false;
    int index = MAdmin.MockAdmins.FindIndex(a => a.id == id);
    if (index != -1)
    {
      MAdmin.MockAdmins[index] = updatedAdmin;
      result = true;
    }
    return result;
  }

  public async Task<bool> DeleteByIdAsync(string id)
  {
    bool result = false;
    int index = MAdmin.MockAdmins.FindIndex(a => a.id == id);
    if (index != -1)
    {
      MAdmin.MockAdmins.RemoveAt(index);
      result = true;
    }
    return result;
  }
}