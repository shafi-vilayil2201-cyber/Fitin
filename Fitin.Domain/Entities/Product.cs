
namespace Fitin.Domain.Entities.Product;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public string Category { get; private set; }
    public int Stock { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;

    private Product() { }

    public Product(string name, decimal price, string category, int stock, string imageUrl = "")
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Category = category;
        Stock = stock;
        ImageUrl = imageUrl;
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
