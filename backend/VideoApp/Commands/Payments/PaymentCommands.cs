using MediatR;
using VideoApp.Common;

namespace VideoApp.Commands.Payments;

public record CreateCheckoutCommand(Guid UserId, Guid VideoId)
    : IRequest<ServiceResult<string>>;

public record ProcessStripeWebhookCommand(string Json, string Signature)
    : IRequest<ServiceResult<bool>>;
