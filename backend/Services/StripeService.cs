using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Threading.Tasks;

namespace backend.Services
{
    public class StripeService
    {
        public StripeService(IConfiguration config)
        {
            var secretKey = config["Stripe:SecretKey"];
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new ArgumentNullException("Stripe secret key missing in config");

            StripeConfiguration.ApiKey = secretKey;
        }

        public async Task<PaymentIntent> CreatePaymentIntent(long amount, string currency = "usd")
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
                PaymentMethodTypes = new[] { "card" }
            };

            var service = new PaymentIntentService();
            return await service.CreateAsync(options);
        }
    }
}
