namespace Fitin.Application.Orders.DTOs;

public class CreateOrderResponseDto
{
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? RazorpayOrderId { get; set; }
}
