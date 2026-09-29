using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using VideoApp.Interfaces;

namespace VideoApp.Services;

public class StripeService : IStripeService
{
    private readonly string _webhookSecret;

    public StripeService(IConfiguration config)
    {
        StripeConfiguration.ApiKey = config["Stripe:SecretKey"]!;
        _webhookSecret = config["Stripe:WebhookSecret"]!;
    }

    public async Task<(string Url, string SessionId)> CreateCheckoutSessionAsync(
        Guid userId, Guid videoId, decimal price, string successUrl, string cancelUrl)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(price * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Premium Video Access"
                        }
                    },
                    Quantity = 1
                }
            },
            Mode = "payment",
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            ClientReferenceId = userId.ToString(),
            Metadata = new Dictionary<string, string>
            {
                { "userId", userId.ToString() },
                { "videoId", videoId.ToString() }
            }
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);
        return (session.Url, session.Id);
    }

    public StripeWebhookResult ParseWebhook(string json, string signature)
    {
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json, signature, _webhookSecret);

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;
                Guid? userId = null;
                Guid? videoId = null;

                if (session?.Metadata?.TryGetValue("userId", out var u) == true)
                    userId = Guid.Parse(u);
                if (session?.Metadata?.TryGetValue("videoId", out var v) == true)
                    videoId = Guid.Parse(v);

                return new StripeWebhookResult(true, stripeEvent.Type, session?.Id, userId, videoId);
            }

            return new StripeWebhookResult(true, stripeEvent.Type, null, null, null);
        }
        catch
        {
            return new StripeWebhookResult(false, null, null, null, null);
        }
    }
}
