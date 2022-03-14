namespace csi5112group1project_service.Models;

public enum OrderStatus
{
  placed,
  pending,
  paid,
  shipping,
  shipped
}

public class PurchasedItem
{
  public string id { get; set; }
  public string productId { get; set; }
  public int quantity { get; set; }

  public PurchasedItem(
    string id,
    string productId,
    int quantity
  )
  {
    this.id = id;
    this.productId = productId;
    this.quantity = quantity;
  }
}

public class Order
{
  public string id { get; set; }
  public long createTime { get; set; } // Unix timestamp
  public OrderStatus status { get; set; }
  public double totalPrice { get; set; }
  public string shippingAddressId { get; set; }
  public List<PurchasedItem> purchasedItems { get; set; }

  public Order(
    string id,
    long createTime,
    OrderStatus status,
    double totalPrice,
    string shippingAddressId,
    List<PurchasedItem> purchasedItems
  )
  {
    this.id = id;
    this.createTime = createTime;
    this.status = status;
    this.totalPrice = totalPrice;
    this.shippingAddressId = shippingAddressId;
    this.purchasedItems = purchasedItems;
  }
}