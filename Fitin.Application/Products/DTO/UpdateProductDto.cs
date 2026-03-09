namespace Fitin.Application.Products.Dto;

public class UpdateProductDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Category { get; set; } = null!;
    public int Stock { get; set; }
}