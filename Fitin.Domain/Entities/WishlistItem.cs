

using Fitin.Domain.Common;

namespace Fitin.Domain.Entities.Wishlists;

public class WishlistItem : BaseEntity
{
    public Guid UserId{get; private set;}
    public Guid ProductId{get; private set;}


    private WishlistItem(){}

    public WishlistItem (Guid userId,Guid productId)
    {
        UserId = userId;
        ProductId = productId;
    }
}