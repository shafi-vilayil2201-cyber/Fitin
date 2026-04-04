namespace Fitin.Application.Orders.DTOs;

public class CreateOrderDto
{
    public string ShippingName { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public string ShippingCity { get; set; } = string.Empty;
    public string ShippingPostalCode { get; set; } = string.Empty;
    public string ShippingPhone { get; set; } = string.Empty;
    public Guid? ProductId { get; set; }
    public int Quantity { get; set; } = 1;
}
