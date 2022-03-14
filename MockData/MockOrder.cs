namespace csi5112group1project_service.MockData;
using csi5112group1project_service.Models;

public class MPurchasedItem
{
  public static readonly List<PurchasedItem> MockPurchasedItems = new List<PurchasedItem> {
    new PurchasedItem(
      id: "1e43972e-5bf4-4d75-8692-de7b7c06bd5d",
      productId: "7",
      quantity: 3
    ),
    new PurchasedItem(
      id: "b36127d4-0d35-414e-a93f-2e1e1d5aca30",
      productId: "2",
      quantity: 1
    ),
    new PurchasedItem(
      id: "d0dfec85-9275-4d81-94e4-1f29f38e9b73",
      productId: "1",
      quantity: 5
    ),
    new PurchasedItem(
      id: "d1a03311-ff66-47da-b283-e5974e990475",
      productId: "13",
      quantity: 2
    ),
    new PurchasedItem(
      id: "964fcf1b-2cf5-445f-b723-6814d85678c6",
      productId: "4",
      quantity: 3
    ),
  };
};

public class MOrder
{
  public static readonly List<Order> MockOrders = new List<Order> {
    new Order(
      id: "2e4c85e1-0772-45bc-8d33-ad56f446ef78",
      createTime: 1616018700000,
      status: OrderStatus.placed,
      totalPrice: 21.28,
      shippingAddressId: "47ed879e-b1b9-4d2b-b4f7-09eed1555bfe",
      purchasedItems: MPurchasedItem.MockPurchasedItems.GetRange(0, 2)
    ),
    new Order(
      id: "6a37eebb-5c97-4fed-8dbb-1632ad7def5b",
      createTime: 1621965241000,
      status: OrderStatus.shipping,
      totalPrice: 137.24,
      shippingAddressId: "99a8af71-1a53-41e1-987e-8c899e8d833f",
      purchasedItems: MPurchasedItem.MockPurchasedItems.GetRange(1, 3)
    ),
    new Order(
      id: "3e4e01f5-1cd8-4762-b4a0-d7ff3003084a",
      createTime: 1639794005000,
      status: OrderStatus.paid,
      totalPrice: 377.75,
      shippingAddressId: "6b034dbd-a299-4fef-9072-31cc21f144db",
      purchasedItems: MPurchasedItem.MockPurchasedItems.GetRange(2, 1)
    ),
    new Order(
      id: "0e0e957e-e586-48ea-b202-4a4cd0589f53",
      createTime: 1638715045000,
      status: OrderStatus.pending,
      totalPrice: 191.69,
      shippingAddressId: "348d51c5-7ec9-4fef-aa13-1dc5b7818572",
      purchasedItems: MPurchasedItem.MockPurchasedItems.GetRange(3, 1)
    ),
    new Order(
      id: "abde0748-cfc6-4abb-8763-84d014524b88",
      createTime: 1624436070000,
      status: OrderStatus.shipped,
      totalPrice: 300.15,
      shippingAddressId: "2e52be1c-7e1e-48d2-bfa2-053db70d9248",
      purchasedItems: MPurchasedItem.MockPurchasedItems.GetRange(4, 1)
    ),
  };
}