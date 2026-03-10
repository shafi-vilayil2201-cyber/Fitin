

namespace Fitin.Domain.Entities.Wishlists;

public class WishlistItem
{
    public Guid Id {get;private set;}
    public Guid UserId{get; private set;}
    public Guid ProductId{get; private set;}
    public DateTime CreatedAt{get; private set;}

    private WishlistItem(){}

    public WishlistItem (Guid userId,Guid productId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ProductId = productId;
        CreatedAt = DateTime.UtcNow;
    }
}