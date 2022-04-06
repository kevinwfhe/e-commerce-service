namespace csi5112group1project_service.Utils;
using csi5112group1project_service.Services;

public class ServicesAggregator
{
  // Mount services to the application.
  public static WebApplicationBuilder AddServices(WebApplicationBuilder builder)
  {
    builder.Services.AddSingleton<ProductService>();
    builder.Services.AddSingleton<OrderService>();
    builder.Services.AddSingleton<ShippingAddressService>();
    builder.Services.AddSingleton<CategoryService>();
    builder.Services.AddSingleton<AuthenticationService>();
    builder.Services.AddSingleton<JwtService>();
    builder.Services.AddSingleton<UserService>();
    builder.Services.AddSingleton<AwsService>();
    builder.Services.AddSingleton<QuestionService>();
    builder.Services.AddSingleton<AnswerService>();
    builder.Services.AddSingleton<CommentService>();
    builder.Services.AddSingleton<DatabaseSettings>(builder.Configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>());
    return builder;
  }
}