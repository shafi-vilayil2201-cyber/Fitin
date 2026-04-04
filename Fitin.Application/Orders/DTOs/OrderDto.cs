using Fitin.Application.Orders.DTOs;

namespace Fitin.Application.Orders.DTOs;

public class OrderDto
{
    public Guid OrderId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }

    // Shipping Details
    public string ShippingName { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public string ShippingCity { get; set; } = string.Empty;
    public string ShippingPostalCode { get; set; } = string.Empty;
    public string ShippingPhone { get; set; } = string.Empty;

    public List<OrderItemDto> Items { get; set; } = new();
}
