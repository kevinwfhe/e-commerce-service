namespace csi5112group1project_service.Models;

public class Client : User
{
  public List<ShippingAddress> shippingAddresses { get; set; }
  // public List<Order> orders { get; set; }
  // public List<Product> wishList { get; set; }
  public Client(
    string id,
    string username,
    string password,
    string emailAddress
  ) : base(id, username, password, emailAddress, role: "client")
  {
    this.shippingAddresses = new List<ShippingAddress>();
    // this.orders = new List<Order>();
    // this.wishList = new List<Product>();
  }
}