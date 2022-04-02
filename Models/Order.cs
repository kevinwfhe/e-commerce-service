namespace csi5112group1project_service.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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

  [BsonRepresentation(BsonType.ObjectId)]
  public string productId { get; set; } // id of the product which the purchased item refers to
  public int quantity { get; set; }

  public PurchasedItem(
    string productId,
    int quantity
  )
  {
    this.productId = productId;
    this.quantity = quantity;
  }
}

public class Order
{

  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string id { get; set; } // unique id of the order itself
  public long createTime { get; set; } // Unix timestamp
  public OrderStatus status { get; set; }
  public double totalPrice { get; set; }

  [BsonRepresentation(BsonType.ObjectId)]
  public string shippingAddressId { get; set; } // id of the shipping address of the order
  public List<PurchasedItem> purchasedItems { get; set; }

  [BsonRepresentation(BsonType.ObjectId)]
  public string userId { get; set; }

  public Order(
    string id,
    long createTime,
    OrderStatus status,
    double totalPrice,
    string shippingAddressId,
    List<PurchasedItem> purchasedItems,
    string userId
  )
  {
    this.id = id;
    this.createTime = createTime;
    this.status = status;
    this.totalPrice = totalPrice;
    this.shippingAddressId = shippingAddressId;
    this.purchasedItems = purchasedItems;
    this.userId = userId;
  }

}
// PurchasedProduct is a result of mapping a PurchasedItem
// to a specific Product with quantity through the productId.
public class PurchasedProduct
{
  public Product product { get; set; }
  public int quantity { get; set; }

  public PurchasedProduct(
    Product product,
    int quantity
  )
  {
    this.product = product;
    this.quantity = quantity;
  }
}

// DetailedOrder is built for constructing a human-readable response upon the
// request of getting an order by its id. Instead of using the shippingAddressId
// and the PurchasedItems, which use ids refering other data documents,
// DetailedOrder should have an instance of ShippingAddress, and a List of PurchasedProduct
// which holds the real information of an address or a product, not an id.
public class DetailedOrder
{

  public string id { get; }
  public long createTime { get; } // Unix timestamp
  public OrderStatus status { get; }
  public double totalPrice { get; }
  public ShippingAddress shippingAddress { get; }
  public List<PurchasedProduct> purchasedProducts { get; }

  public DetailedOrder(
  string id,
  long createTime,
  OrderStatus status,
  double totalPrice,
  ShippingAddress shippingAddress,
  List<PurchasedProduct> purchasedProducts
)
  {
    this.id = id;
    this.createTime = createTime;
    this.status = status;
    this.totalPrice = totalPrice;
    this.shippingAddress = shippingAddress;
    this.purchasedProducts = purchasedProducts;
  }
}