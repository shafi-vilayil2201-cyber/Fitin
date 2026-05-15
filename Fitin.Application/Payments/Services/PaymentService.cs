using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using Fitin.Application.Payments.Interfaces;

namespace Fitin.Application.Payments.Services;

public class PaymentService : IPaymentService
{
    private readonly string _keyId;
    private readonly string _keySecret;

    public PaymentService(IConfiguration configuration)
    {
        _keyId = configuration["Razorpay:KeyId"] ?? throw new ArgumentNullException("Razorpay KeyId is missing");
        _keySecret = configuration["Razorpay:KeySecret"] ?? throw new ArgumentNullException("Razorpay KeySecret is missing");
    }

    public async Task<string> CreateOrderAsync(decimal amount, string receiptId)
    {
        return await Task.Run(() => {
            var client = new RazorpayClient(_keyId, _keySecret);

            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", (long)(amount * 100)); // amount in the smallest currency unit (paise)
            options.Add("receipt", receiptId);
            options.Add("currency", "INR");
            options.Add("payment_capture", "1"); // 1 for auto capture

            Order order = client.Order.Create(options);
            return order["id"].ToString();
        });
    }

    public bool VerifyPayment(string orderId, string paymentId, string signature)
    {
        try
        {
            var attributes = new Dictionary<string, string>
            {
                { "razorpay_order_id", orderId },
                { "razorpay_payment_id", paymentId },
                { "razorpay_signature", signature }
            };

            Utils.verifyPaymentSignature(attributes);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
