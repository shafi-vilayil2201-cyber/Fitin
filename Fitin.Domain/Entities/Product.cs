
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

    private Product() { } 

    public Product(string name, decimal price,Guid categoryId, int stock, string imageUrl = "")
    {
        Name = name;
        Price = price;
        CategoryId = categoryId;
        Stock = stock;
        ImageUrl = imageUrl;
    }

    public void UpdateDetails(string name, decimal price,Guid categoryId, int stock, string imageUrl = "")
    {
        Name = name;
        Price = price;
        CategoryId = categoryId;
        Stock = stock;
        ImageUrl = imageUrl;
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
