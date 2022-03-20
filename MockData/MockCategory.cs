namespace csi5112group1project_service.MockData;
using csi5112group1project_service.Models;

public class MCategory
{
  public static readonly List<Category> MockCategory = new List<Category> {
    new Category(
      id: "1",
      name: "Electronic"
    ),
    new Category(
      id: "2",
      name: "Shoes"
    ),
    new Category(
      id: "3",
      name: "Toy"
    ),
    new Category(
      id: "4",
      name: "Clothes"
    ),
    new Category(
      id: "5",
      name: "Food"
    )
  };
}