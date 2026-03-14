
namespace Fitin.Application.Cart.Dto;

public class AddToCartDto
{
    public Guid ProductId {get; set;}
}

public class CartItemDto
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal ProductPrice { get; set; }

    public string ProductImageUrl { get; set; } = string.Empty;

    public int Quantity { get; set; }
}