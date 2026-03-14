

using Fitin.Domain.Common;
using Fitin.Domain.Entities.Products;

namespace Fitin.Domain.Entities.CartItems;

public class CartItem : BaseEntity
{

    public Guid UserId { get; private set; }

    public Guid ProductId { get; private set; }

    public Product Product { get; private set; } = null!;

    public int Quantity { get; set; }


    private CartItem() {}

    public CartItem(Guid userId, Guid productId)
    {
        UserId = userId;
        ProductId = productId;
        Quantity = 1;

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