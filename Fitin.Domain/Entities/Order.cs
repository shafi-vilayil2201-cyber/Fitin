using Fitin.Domain.Common;

namespace Fitin.Domain.Entities;

public class Order : BaseEntity
{
   
    public Guid UserId{get; private set;}
    public decimal TotalAmount {get; private set;}
    public string Status {get;private set;} = "Pending";
    public string ShippingName { get; private set; } = string.Empty;
    public string ShippingAddress { get; private set; } = string.Empty;
    public string ShippingCity { get; private set; } = string.Empty;
    public string ShippingPostalCode { get; private set; } = string.Empty;
    public string ShippingPhone { get; private set; } = string.Empty;

    public string? RazorpayOrderId { get; private set; }
    public string? RazorpayPaymentId { get; private set; }
    public string? RazorpaySignature { get; private set; }
    public string PaymentStatus { get; private set; } = "Pending";

    public ICollection<OrderItem> OrderItems {get; private set;} = new List<OrderItem>();

    private Order(){}
    public Order(Guid userId, decimal totalAmount, string shippingName, string shippingAddress, string shippingCity, string shippingPostalCode, string shippingPhone)
    {
        UserId = userId;
        TotalAmount = totalAmount;
        Status = "Pending";
        PaymentStatus = "Pending";
        ShippingName = shippingName;
        ShippingAddress = shippingAddress;
        ShippingCity = shippingCity;
        ShippingPostalCode = shippingPostalCode;
        ShippingPhone = shippingPhone;
    }

    public void SetRazorpayOrderId(string orderId)
    {
        RazorpayOrderId = orderId;
        MarkUpdated();
    }

    public void MarkAsPaid(string paymentId, string signature)
    {
        RazorpayPaymentId = paymentId;
        RazorpaySignature = signature;
        PaymentStatus = "Paid";
        Status = "Processing";
        MarkUpdated();
    }

    public void MarkAsFailed()
    {
        PaymentStatus = "Failed";
        Status = "Cancelled";
        MarkUpdated();
    }


    public void AddOrderItem(OrderItem orderItem)
    {
        OrderItems.Add(orderItem);
    }
    public void UpdateStatus(string newStatus)
    {
        if (Status == "Delivered" || Status == "Cancelled")
        {
            throw new InvalidOperationException($"Cannot change status from terminal state: {Status}");
        }

        bool isValid = Status switch
        {
            "Pending" => newStatus == "Processing" || newStatus == "Cancelled",
            "Processing" => newStatus == "Delivered" || newStatus == "Cancelled",
            _ => false
        };

        if (!isValid)
        {
            throw new InvalidOperationException($"Invalid status transition from {Status} to {newStatus}");
        }

        Status = newStatus;
        MarkUpdated();
    }

}