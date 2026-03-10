



namespace Fitin.Domain.Entities.CartItems;

public class CartItem
{
    public Guid Id{get;  set;}
    public Guid UserId{get;  set;}
    public Guid ProductId{get; set;}
    public int Quantity {get; set; }
    public DateTime CreatedAt{get; set;}

    // private CartItem() {}

    public CartItem(Guid userId,Guid productId,int quantity,DateTime createdAt)
    {
        Id= Guid.NewGuid();
        UserId = userId;
        ProductId = productId;
        Quantity = quantity;
        CreatedAt = createdAt;
    }

}