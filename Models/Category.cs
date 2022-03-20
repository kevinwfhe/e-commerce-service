namespace csi5112group1project_service.Models;

public class Category
{
  public String id { get; set; }
  public String name { get; set; }

  public Category(
    string id,
    string name
  )
  {
    this.id = id;
    this.name = name;
  }

}