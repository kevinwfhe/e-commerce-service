namespace csi5112group1project_service.MockData;
using csi5112group1project_service.Models;

public class MProduct
{
  private static readonly string MockDescription =
    "BEST SHOES EVER.\n A shoe is an item of footwear intended to protect and comfort the human foot. Shoes are also used as an item of decoration and fashion. ... Traditionally, shoes have been made from leather, wood or canvas, but are increasingly being made from rubber, plastics, and other petrochemical-derived materials.";
  public static readonly List<Product> MockProducts = new List<Product> {
    new Product(
      id: "1",
      title: "Addis Runner",
      price: 233,
      description: MockDescription,
      image: "images/shoe1.png",
      category: "shoe"
    ),
    new Product(
      id: "2",
      title: "NB runner",
      price: 198,
      description: MockDescription,
      image: "images/shoe2.png",
      category: "shoe"
    ),
    new Product(
      id: "3",
      title: "Nike Runner",
      price: 456,
      description: MockDescription,
      image: "images/shoe3.png",
      category: "shoe"
    ),
    new Product(
      id: "4",
      title: "Champion Runner",
      price: 150,
      description: MockDescription,
      image: "images/shoe4.png",
      category: "shoe"
    ),
    new Product(
      id: "5",
      title: "Addis sliper",
      price: 99,
      description: MockDescription,
      image: "images/shoe5.png",
      category: "shoe"
    ),
    new Product(
        id: "6",
        title: "Baby hooded sweater",
        price: 79,
        description: MockDescription,
        image: "images/cloth1.png",
        category: "clothes"
    ),
    new Product(
        id: "7",
        title: "Baby cloth",
        price: 45,
        description: MockDescription,
        image: "images/cloth2.png",
        category: "clothes"
    ),
    new Product(
        id: "8",
        title: "High rise pant",
        price: 199,
        description: MockDescription,
        image: "images/cloth3.png",
        category: "clothes"
    ),
    new Product(
        id: "9",
        title: "Mountain jacket",
        price: 46,
        description: MockDescription,
        image: "images/cloth4.png",
        category: "clothes"
    ),
    new Product(
        id: "10",
        title: "Spider jacket",
        price: 666,
        description: MockDescription,
        image: "images/cloth5.png",
        category: "clothes"
    ),
    new Product(
        id: "11",
        title: "Samsung Computer",
        price: 249,
        description: MockDescription,
        image: "images/electric1.png",
        category: "electronic"
    ),
    new Product(
        id: "12",
        title: "Samsung Watch",
        price: 229,
        description: MockDescription,
        image: "images/electric2.png",
        category: "electronic"
    ),
    new Product(
        id: "13",
        title: "Canon EOS R5 Camera",
        price: 4999,
        description: MockDescription,
        image: "images/electric3.png",
        category: "electronic"
    ),
    new Product(
        id: "14",
        title: "Nintendo Switch",
        price: 319,
        description: MockDescription,
        image: "images/electric4.png",
        category: "electronic"
    ),
    new Product(
        id: "15",
        title: "Guild Guitar",
        price: 666,
        description: MockDescription,
        image: "images/electric5.png",
        category: "electronic"
    ),
    new Product(
        id: "16",
        title: "Jellycat Cloud Plush",
        price: 24,
        description: MockDescription,
        image: "images/toy1.png",
        category: "toy"
    ),
    new Product(
        id: "17",
        title: "Hill's Science Dog Food",
        price: 68,
        description: MockDescription,
        image: "images/food1.png",
        category: "food"
    ),
  };
}
