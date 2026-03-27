
namespace Fitin.Application.Products.Dto;

public class ProductDto
{
    public Guid Id{get; set;}
    public string Name {get; set;} = string.Empty;
    public decimal Price {get; set;}
    public Guid CategoryId {get; set;}
    public string CategoryName{get; set;} = string.Empty;
    public int Stock {get; set;}
    public string ImageUrl { get; set; } = string.Empty;

}
