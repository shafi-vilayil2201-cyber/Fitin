

using Fitin.Domain.Entities.Products;

namespace Fitin.Domain.Entities.CartItems;

public class CartItem
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid ProductId { get; private set; }

    public Product Product { get; private set; } = null!;

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; private set; }

    // private CartItem() {}

    public CartItem(Guid userId, Guid productId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ProductId = productId;
        Quantity = 1;
        CreatedAt = DateTime.UtcNow;
    }

    public void IncreaseQuantity()
    {
        Quantity++;
    }

    public void DecreaseQuantity()
    {
        if (Quantity > 1)
            Quantity--;
    }
}