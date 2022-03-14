namespace csi5112group1project_service.Services;
using csi5112group1project_service.MockData;
using csi5112group1project_service.Models;
public class OrderService
{
  public OrderService() { }
  public async Task<string> CreateAsync(Order newOrder)
  {
    MOrder.MockOrders.Add(newOrder);
    return newOrder.id;
  }
  public async Task<List<Order>> GetAsync()
  {
    return MOrder.MockOrders;
  }

  public async Task<Order> GetByIdAsync(string id)
  {
    return MOrder.MockOrders.Find(a => a.id == id);
  }

  public async Task<bool> UpdateByIdAsync(string id, Order updateOrder)
  {
    bool result = false;
    int index = MOrder.MockOrders.FindIndex(a => a.id == id);
    if (index != -1)
    {
      MOrder.MockOrders[index] = updateOrder;
      result = true;
    }
    return result;
  }

  public async Task<bool> DeleteByIdAsync(string id)
  {
    bool result = false;
    int index = MOrder.MockOrders.FindIndex(a => a.id == id);
    if (index != -1)
    {
      MOrder.MockOrders.RemoveAt(index);
      result = true;
    }
    return result;
  }
}