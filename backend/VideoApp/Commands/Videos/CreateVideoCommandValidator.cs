using FluentValidation;

namespace VideoApp.Commands.Videos;

public class CreateVideoCommandValidator : AbstractValidator<CreateVideoCommand>
{
    public CreateVideoCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(255);

        RuleFor(x => x.Description)
            .MaximumLength(2000);

        RuleFor(x => x.MuxUploadId)
            .NotEmpty().WithMessage("Mux upload ID is required.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .When(x => x.IsPremium)
            .WithMessage("Price must be non-negative for premium videos.");
    }
}
