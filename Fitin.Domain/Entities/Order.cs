using Fitin.Domain.Common;

namespace Fitin.Domain.Entities;

public class Order : BaseEntity
{
   
    public Guid UserId{get; private set;}
    public decimal TotalAmount {get; private set;}
    public string Status {get;private set;} = "Pending";

    public ICollection<OrderItem> OrderItems {get; private set;} = new List<OrderItem>();

    private Order(){}

    public Order(Guid userId,decimal totalAmount)
    {
        UserId = userId;
        TotalAmount  = totalAmount;
        Status = "Pending";
    }

    public void AddOrderItem(OrderItem orderItem)
    {
        OrderItems.Add(orderItem);
    }
    public void UpdateStatus(string status)
    {
        Status = status;
        MarkUpdated();
    }

}