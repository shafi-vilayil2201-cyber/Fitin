

using Fitin.Domain.Common;
using Fitin.Domain.Entities.Products;

namespace Fitin.Domain.Entities.Wishlists;

public class WishlistItem : BaseEntity
{
    public Guid UserId{get; private set;}
    public Guid ProductId{get; private set;}
    public Product Product { get; private set; } = null!;



    private WishlistItem(){}

    public WishlistItem (Guid userId,Guid productId)
    {
        UserId = userId;
        ProductId = productId;
    }
}
