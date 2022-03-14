namespace csi5112group1project_service.Utils;
using csi5112group1project_service.Services;

public class ServicesAggregator {
  // Mount services to the application.
  public static WebApplicationBuilder AddServices(WebApplicationBuilder builder) {
    builder.Services.AddSingleton<AdminService>();
    builder.Services.AddSingleton<ClientService>();
    builder.Services.AddSingleton<ProductService>();
    builder.Services.AddSingleton<OrderService>();
    builder.Services.AddSingleton<ShippingAddressService>();
    return builder;
  }
}