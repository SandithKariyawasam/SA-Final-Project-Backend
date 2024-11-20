// PaymentService.cs
using AutoBid.Services;
using Stripe;

public class PaymentService : IPaymentService
{
    private readonly IConfiguration _config;

    public PaymentService(IConfiguration config)
    {
        _config = config;
        StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
    }

    public async Task<string> CreatePaymentIntent(decimal amount, string currency)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100), // Convert amount to cents
            Currency = currency
        };
        var service = new PaymentIntentService();
        var paymentIntent = await service.CreateAsync(options);
        return paymentIntent.ClientSecret;
    }
}