
using Fitin.Domain.Common;
using Fitin.Domain.Entities.Categories;

namespace Fitin.Domain.Entities.Products;

public class Product : BaseEntity
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public Guid CategoryId{get;private set;}
    public Category Category{get ; private set;} = null!;
    public int Stock { get; private set; }
    public bool IsOutOfStock => Stock <= 0;
    public string ImageUrl { get; private set; } = string.Empty;
    public string Brand { get; private set; } = string.Empty;
    public string Sport { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string ShortDescription { get; private set; } = string.Empty;
    public string LongDescription { get; private set; } = string.Empty;
    public decimal Rating { get; private set; }
    public decimal Discount { get; private set; }

    private Product() { } 

    public Product(
        string name,
        decimal price,
        Guid categoryId,
        int stock,
        string imageUrl = "",
        string brand = "",
        string sport = "",
        string description = "",
        string shortDescription = "",
        string longDescription = "",
        decimal rating = 0,
        decimal discount = 0)
    {
        Name = name;
        Price = price;
        CategoryId = categoryId;
        Stock = stock;
        ImageUrl = imageUrl;
        Brand = brand;
        Sport = sport;
        Description = description;
        ShortDescription = shortDescription;
        LongDescription = longDescription;
        Rating = rating;
        Discount = discount;
    }

    public void UpdateDetails(
        string name,
        decimal price,
        Guid categoryId,
        int stock,
        string imageUrl = "",
        string brand = "",
        string sport = "",
        string description = "",
        string shortDescription = "",
        string longDescription = "",
        decimal rating = 0,
        decimal discount = 0)
    {
        Name = name;
        Price = price;
        CategoryId = categoryId;
        Stock = stock;
        ImageUrl = imageUrl;
        Brand = brand;
        Sport = sport;
        Description = description;
        ShortDescription = shortDescription;
        LongDescription = longDescription;
        Rating = rating;
        Discount = discount;
        MarkUpdated();
    }

    public void ReduceStock(int quantity)
    {
        if (Stock < quantity)
            throw new Exception("Not enough stock");

        Stock -= quantity;
    }

    public void IncreaseStock(int quantity)
    {
        Stock += quantity;
    }
}
