
namespace AutoBid.Services
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentIntent(decimal amount, string currency);
    }
}
