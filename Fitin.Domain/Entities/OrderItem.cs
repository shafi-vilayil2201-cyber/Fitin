

using Fitin.Domain.Common;
using Fitin.Domain.Entities.Products;

namespace Fitin.Domain.Entities;

public class OrderItem : BaseEntity
{
    public Guid OrderId{get; private set;}
    public Guid ProductId{get; private set;}

    public string ProductName{get; private set;} = string.Empty;
    public string ProductImageUrl{get; private set;} =string.Empty;
    public decimal UnitPrice{get; private set;}
    public int Quantity {get; private set;}

    public Order Order{get;private set;} = null!;
    public Product Product{get; private set;} = null!;

    private OrderItem(){}

    // Move productImageUrl to the 4th position
    public OrderItem(Guid orderId, Guid productId, string productName, string productImageUrl, decimal unitPrice, int quantity)
    {
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName;
        ProductImageUrl = productImageUrl; // This matches the 4th argument now
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public decimal GetTotal()
    {
        return UnitPrice * Quantity;
    }

}