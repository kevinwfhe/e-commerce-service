namespace csi5112group1project_service.Services;
using csi5112group1project_service.MockData;
using csi5112group1project_service.Models;
public class OrderService
{
  public OrderService() { }
  public async Task<Order> CreateAsync(Order newOrder)
  {
    var _id = Guid.NewGuid();
    newOrder.id = _id.ToString();
    var _purchasedItems = newOrder.purchasedItems;
    // generate unique id for each purchasedItem record
    _purchasedItems.ForEach((pi) => pi.id = Guid.NewGuid().ToString());
    MOrder.MockOrders.Add(newOrder);
    return newOrder;
  }
  public async Task<List<Order>> GetAsync()
  {
    return MOrder.MockOrders;
  }

  public async Task<List<Order>> GetByKeywordAsync(string keyword)
  {
    // keyword only supports orderId for now
    return MOrder.MockOrders.FindAll((o) => o.id.Contains(keyword));
  }

  public List<Order> SortOrders(List<Order> orders, string sortIndex, string sortAsc)
  {
    if (orders.Count == 0 || sortIndex == null) return orders;
    var _sortIndex = int.Parse(sortIndex);
    var _sortAsc = int.Parse(sortAsc);
    List<Order> dataSorted;
    if (_sortIndex == 1) // sort by order status
    {
      dataSorted = _sortAsc == 1
        ? orders.OrderBy(d => d.status).ToList()
        : orders.OrderByDescending(d => d.status).ToList();
    }
    else if (_sortIndex == 2) // sort by order create time
    {
      dataSorted = _sortAsc == 1
        ? orders.OrderBy(d => d.createTime).ToList()
        : orders.OrderByDescending(d => d.createTime).ToList();
    }
    else if (_sortIndex == 3) // sort by total price
    {
      dataSorted = _sortAsc == 1
        ? orders.OrderBy(d => d.totalPrice).ToList()
        : orders.OrderByDescending(d => d.totalPrice).ToList();
    }
    else
    {
      dataSorted = orders;
    }
    return dataSorted;
  }

  public PageInfo<List<Order>> PaginateOrders(List<Order> orders, string offset, string pageSize)
  {
    if (offset == null || pageSize == null)
    {
      return new PageInfo<List<Order>>(rows: orders, totalRows: orders.Count);
    }
    var _offset = int.Parse(offset);
    var _pageSize = int.Parse(pageSize);
    int totalRows = orders.Count;
    var dataInRange = _offset + _pageSize > totalRows
      ? orders.GetRange(_offset, totalRows - _offset)
      : orders.GetRange(_offset, _pageSize);
    return new PageInfo<List<Order>>(rows: dataInRange, totalRows: totalRows);
  }

  // When get an order by its id, details of the order should be retured in human-readable way
  // i.e. Detailed shipping address instead of shippingAddressId
  // i.e. Detailed products instead of productIds
  public async Task<DetailedOrder> GetByIdAsync(string id)
  {
    ShippingAddressService _addressService = new ShippingAddressService();
    ProductService _productService = new ProductService();

    // Get order by id
    var order = MOrder.MockOrders.Find(a => a.id == id);
    // Get shipping address of this order by its shippingAddressId
    var shippingAddress = await _addressService.GetByIdAsync(order.shippingAddressId);
    // Get all the purchased products of this order by the productId of its putchasedItems
    var purchasedProducts = await Task.WhenAll(
       order.purchasedItems
       .Select(item => _productService
         .GetByIdAsync(item.productId)
         .ContinueWith(product => new PurchasedProduct(item.id, product.Result, item.quantity)))
       );
    // assemble the DetailedOrder with all the components
    return new DetailedOrder(
      id: order.id,
      createTime: order.createTime,
      status: order.status,
      totalPrice: order.totalPrice,
      shippingAddress: shippingAddress,
      purchasedProducts: purchasedProducts.ToList()
    );
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