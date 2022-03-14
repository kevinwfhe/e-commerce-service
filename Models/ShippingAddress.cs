namespace csi5112group1project_service.Models;

public class ShippingAddress
{
  public string id { get; set; }
  public string fullname { get; set; }
  public string phoneNumber { get; set; }
  public string addressFirstLine { get; set; }
  public string addressSecondLine { get; set; }
  public string city { get; set; }
  public string province { get; set; }
  public string postalCode { get; set; }

  public ShippingAddress(
    string id,
    string fullname,
    string phoneNumber,
    string addressFirstLine,
    string addressSecondLine,
    string city,
    string province,
    string postalCode
  )
  {
    this.id = id;
    this.fullname = fullname;
    this.phoneNumber = phoneNumber;
    this.addressFirstLine = addressFirstLine;
    this.addressSecondLine = addressSecondLine;
    this.city = city;
    this.province = province;
    this.postalCode = postalCode;
  }
}