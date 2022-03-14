namespace csi5112group1project_service.Services;
using csi5112group1project_service.MockData;
using csi5112group1project_service.Models;

public class ClientService
{
  public ClientService() { }
  public async Task<string> CreateAsync(Client newClient)
  {
    MClient.MockClients.Add(newClient);
    return newClient.id;
  }

  public async Task<List<Client>> GetAsync()
  {
    return MClient.MockClients;
  }

  public async Task<Client> GetByIdAsync(string id)
  {
    return MClient.MockClients.Find(p => p.id == id);
  }

  public async Task<bool> UpdateByIdAsync(string id, Client updatedClient)
  {
    bool result = false;
    int index = MClient.MockClients.FindIndex(p => p.id == id);
    if (index != -1)
    {
      MClient.MockClients[index] = updatedClient;
      result = true;
    }
    return result;
  }

  public async Task<bool> DeleteByIdAsync(string id)
  {
    bool result = false;
    int index = MClient.MockClients.FindIndex(p => p.id == id);
    if (index != -1)
    {
      MClient.MockClients.RemoveAt(index);
      result = true;
    }
    return result;
  }
}