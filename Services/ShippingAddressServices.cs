namespace csi5112group1project_service.Services;
using csi5112group1project_service.MockData;
using csi5112group1project_service.Models;
public class ShippingAddressService
{
  public ShippingAddressService() { }
  public async Task<ShippingAddress> CreateAsync(ShippingAddress newAddress)
  {
    Guid _id = Guid.NewGuid();
    newAddress.id = _id.ToString();
    MShippingAddress.MockShippingAddresses.Add(newAddress);
    return newAddress;
  }
  public async Task<List<ShippingAddress>> GetAsync()
  {
    return MShippingAddress.MockShippingAddresses;
  }

  public async Task<ShippingAddress> GetByIdAsync(string id)
  {
    return MShippingAddress.MockShippingAddresses.Find(a => a.id == id);
  }

  public async Task<bool> UpdateByIdAsync(string id, ShippingAddress updatedAddress)
  {
    bool result = false;
    int index = MShippingAddress.MockShippingAddresses.FindIndex(a => a.id == id);
    if (index != -1)
    {
      MShippingAddress.MockShippingAddresses[index] = updatedAddress;
      result = true;
    }
    return result;
  }

  public async Task<bool> DeleteByIdAsync(string id)
  {
    bool result = false;
    int index = MShippingAddress.MockShippingAddresses.FindIndex(a => a.id == id);
    if (index != -1)
    {
      MShippingAddress.MockShippingAddresses.RemoveAt(index);
      result = true;
    }
    return result;
  }
}