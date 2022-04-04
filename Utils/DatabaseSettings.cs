namespace csi5112group1project_service.Utils;

public class DatabaseSettings
{
  public string ConnectionString { get; set; } = null!;
  public string DatabaseName { get; set; } = null!;

  public string UserCollectionName { get; set; } = null!;
  public string ProductCollectionName { get; set; } = null!;
  public string OrderCollectionName { get; set; } = null!;
  public string CategoryCollectionName { get; set; } = null!;
  public string ShippingAddressCollectionName { get; set; } = null!;

  public string DeletedProductCollectionName { get; set; } = null!;
  public string DeletedCategoryCollectionName { get; set; } = null!;
  public string DeletedShippingAddressCollectionName { get; set; } = null!;
  public string BlacklistTokenCollectionName { get; set; } = null!;

}