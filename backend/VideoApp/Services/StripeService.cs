using Microsoft.Extensions.Configuration;
using Stripe;
using System.Collections.Generic;

namespace VideoApp.Services
{
    public class StripeService
    {
        private readonly IConfiguration _config;

        public StripeService(IConfiguration config)
        {
            _config = config;
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }

        public PaymentIntent CreatePaymentIntent(long amount, string currency)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
                PaymentMethodTypes = new List<string> { "card" }
            };

            var service = new PaymentIntentService();
            return service.Create(options);
        }
    }
}
