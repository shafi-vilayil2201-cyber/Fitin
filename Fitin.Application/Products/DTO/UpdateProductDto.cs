namespace Fitin.Application.Products.Dto;

public class UpdateProductDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public int Stock { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}
