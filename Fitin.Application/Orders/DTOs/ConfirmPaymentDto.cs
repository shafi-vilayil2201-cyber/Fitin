namespace Fitin.Application.Orders.DTOs;

public class ConfirmPaymentDto
{
    public Guid OrderId { get; set; }
    public string RazorpayPaymentId { get; set; } = string.Empty;
    public string RazorpayOrderId { get; set; } = string.Empty;
    public string RazorpaySignature { get; set; } = string.Empty;
}
