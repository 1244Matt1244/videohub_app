namespace VideoApp.Interfaces;

public interface IStripeService
{
    Task<(string Url, string SessionId)> CreateCheckoutSessionAsync(
        Guid userId, Guid videoId, decimal price,
        string successUrl, string cancelUrl);

    StripeWebhookResult ParseWebhook(string json, string signature);
}

public record StripeWebhookResult(
    bool IsValid, string? EventType, string? SessionId,
    Guid? UserId, Guid? VideoId);
