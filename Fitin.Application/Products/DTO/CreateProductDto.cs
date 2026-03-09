namespace Fitin.Application.Products.Dto;

public class CreateProductDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Category { get; set; } = null!;
    public int Stock { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}