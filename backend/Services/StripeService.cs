using Stripe;
using System.Threading.Tasks;

namespace backend.Services
{
    public class StripeService
    {
        private readonly PaymentIntentService _paymentIntentService;

        public StripeService()
        {
            StripeConfiguration.ApiKey = "YOUR_STRIPE_SECRET_KEY";
            _paymentIntentService = new PaymentIntentService();
        }

        public async Task<PaymentIntent> CreatePaymentIntent(long amount, string currency)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
                PaymentMethodTypes = new List<string> { "card" }
            };

            var paymentIntent = await _paymentIntentService.CreateAsync(options);
            return paymentIntent;
        }
    }
}
