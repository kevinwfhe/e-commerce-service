namespace csi5112group1project_service.MockData;
using csi5112group1project_service.Models;

public class MShippingAddress
{
  public static readonly List<ShippingAddress> MockShippingAddresses = new List<ShippingAddress> {
    new ShippingAddress(
      id: "47ed879e-b1b9-4d2b-b4f7-09eed1555bfe",
      fullname: "Siouxie Pepi",
      phoneNumber: "1859541196",
      addressFirstLine: "51996 Holmberg Crossing",
      addressSecondLine: "805 Laurel Alley",
      city: "Xinbao",
      province: "Balakhninskiy",
      postalCode: "Q43 H63"
    ),
    new ShippingAddress(
      id: "99a8af71-1a53-41e1-987e-8c899e8d833f",
      fullname: "Verena Markus",
      phoneNumber: "6898557928",
      addressFirstLine: "0757 Oak Valley Place",
      addressSecondLine: "006 Graceland Lane",
      city: "Landerneau",
      province: "Stockholm",
      postalCode: "K93 G43"
    ),
    new ShippingAddress(
      id: "6b034dbd-a299-4fef-9072-31cc21f144db",
      fullname: "Jobey Bolsover",
      phoneNumber: "1327839401",
      addressFirstLine: "956 Declaration Place",
      addressSecondLine: "5 Shasta Court",
      city: "Onueke",
      province: "Bang Mun Nak",
      postalCode: "O83 Z53"
    ),
    new ShippingAddress(
      id: "348d51c5-7ec9-4fef-aa13-1dc5b7818572",
      fullname: "Alwyn Humbell",
      phoneNumber: "9042632337",
      addressFirstLine: "8850 Loftsgordon Court",
      addressSecondLine: "39 Jay Crossing",
      city: "Limoges",
      province: "Yamoussoukro",
      postalCode: "T13 D23"
    ),
    new ShippingAddress(
      id: "2e52be1c-7e1e-48d2-bfa2-053db70d9248",
      fullname: "Hayes Cottel",
      phoneNumber: "1906802327",
      addressFirstLine: "59 Pankratz Street",
      addressSecondLine: "3 Carey Plaza",
      city: "Hexia",
      province: "Amiens",
      postalCode: "P53 Z73"
    )
  };
}