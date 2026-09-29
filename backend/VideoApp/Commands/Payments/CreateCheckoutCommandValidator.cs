using FluentValidation;

namespace VideoApp.Commands.Payments;

public class CreateCheckoutCommandValidator : AbstractValidator<CreateCheckoutCommand>
{
    public CreateCheckoutCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.VideoId).NotEmpty();
    }
}
