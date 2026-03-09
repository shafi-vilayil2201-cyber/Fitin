



namespace Fitin.Domain.Entities.CartItem;

public class CartItem
{
    public Guid Id{get; private set;}
    public Guid UserId{get; private set;}
    public Guid ProductId{get;private set;}
    public int Quantity {get; private set; }
    public DateTime CreatedAt{get;private set;}

    private CartItem() {}

    public CartItem(Guid userId,Guid productId,int quantity,DateTime createdAt)
    {
        Id= Guid.NewGuid();
        ProductId = productId;
        Quantity = quantity;
        CreatedAt = createdAt;
    }

}