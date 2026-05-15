namespace Fitin.Application.Payments.Interfaces;

public interface IPaymentService
{
    Task<string> CreateOrderAsync(decimal amount, string receiptId);
    bool VerifyPayment(string orderId, string paymentId, string signature);
}
