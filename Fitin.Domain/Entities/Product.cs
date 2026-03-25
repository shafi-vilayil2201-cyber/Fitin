
using Fitin.Domain.Common;

namespace Fitin.Domain.Entities.Products;

public class Product : BaseEntity
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public string Category { get; private set; }
    public int Stock { get; private set; }
    public bool IsOutOfStock => Stock <= 0;
    public string ImageUrl { get; private set; } = string.Empty;

    private Product() { }

    public Product(string name, decimal price, string category, int stock, string imageUrl = "")
    {
        Name = name;
        Price = price;
        Category = category;
        Stock = stock;
        ImageUrl = imageUrl;
    }

    public void UpdateDetails(string name, decimal price, string category, int stock, string imageUrl = "")
    {
        Name = name;
        Price = price;
        Category = category;
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
