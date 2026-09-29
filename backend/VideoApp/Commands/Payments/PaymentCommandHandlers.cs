using MediatR;
using Microsoft.Extensions.Configuration;
using VideoApp.Common;
using VideoApp.Interfaces;
using VideoApp.Domain.Entities;

namespace VideoApp.Commands.Payments;

public class CreateCheckoutCommandHandler : IRequestHandler<CreateCheckoutCommand, ServiceResult<string>>
{
    private readonly IVideoRepository _videos;
    private readonly IPaymentRepository _payments;
    private readonly IStripeService _stripe;
    private readonly IConfiguration _config;

    public CreateCheckoutCommandHandler(
        IVideoRepository videos, IPaymentRepository payments,
        IStripeService stripe, IConfiguration config)
    {
        _videos = videos; _payments = payments; _stripe = stripe; _config = config;
    }

    public async Task<ServiceResult<string>> Handle(CreateCheckoutCommand request, CancellationToken ct)
    {
        var video = await _videos.GetByIdAsync(request.VideoId);
        if (video is null) return ServiceResult<string>.Fail("Video not found");
        if (!video.IsPremium) return ServiceResult<string>.Fail("Video is not premium");

        var frontend = _config["Frontend:Url"];
        var (url, sessionId) = await _stripe.CreateCheckoutSessionAsync(
            request.UserId, video.Id, video.Price,
            $"{frontend}/payment/success", $"{frontend}/payment/cancel");

        await _payments.CreateAsync(new Payment
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            VideoId = video.Id,
            StripeSessionId = sessionId,
            Amount = video.Price,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        });

        return ServiceResult<string>.Ok(url);
    }
}

public class ProcessStripeWebhookCommandHandler : IRequestHandler<ProcessStripeWebhookCommand, ServiceResult<bool>>
{
    private readonly IStripeService _stripe;
    private readonly IUserRepository _users;
    private readonly IPaymentRepository _payments;

    public ProcessStripeWebhookCommandHandler(
        IStripeService stripe, IUserRepository users, IPaymentRepository payments)
    {
        _stripe = stripe; _users = users; _payments = payments;
    }

    public async Task<ServiceResult<bool>> Handle(ProcessStripeWebhookCommand request, CancellationToken ct)
    {
        var result = _stripe.ParseWebhook(request.Json, request.Signature);
        if (!result.IsValid) return ServiceResult<bool>.Fail("Invalid signature");

        if (result.EventType == "checkout.session.completed"
            && result.UserId.HasValue && result.SessionId is not null)
        {
            await _payments.UpdateStatusAsync(result.SessionId, "completed");

            var user = await _users.GetByIdAsync(result.UserId.Value);
            if (user is not null)
            {
                user.IsPremium = true;
                user.PremiumExpiresAt = DateTime.UtcNow.AddMonths(1);
                await _users.UpdateAsync(user);
            }
        }

        return ServiceResult<bool>.Ok(true);
    }
}
