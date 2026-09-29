using FluentValidation.TestHelper;
using VideoApp.Commands.Videos;
using Xunit;

namespace VideoApp.Tests;

public class CreateVideoCommandValidatorTests
{
    private readonly CreateVideoCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Title_Empty()
    {
        var result = _validator.TestValidate(
            new CreateVideoCommand(Guid.NewGuid(), "", null, "upload", false, 0));
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_UploadId_Empty()
    {
        var result = _validator.TestValidate(
            new CreateVideoCommand(Guid.NewGuid(), "Title", null, "", false, 0));
        result.ShouldHaveValidationErrorFor(x => x.MuxUploadId);
    }

    [Fact]
    public void Should_Have_Error_When_Premium_With_Negative_Price()
    {
        var result = _validator.TestValidate(
            new CreateVideoCommand(Guid.NewGuid(), "Title", null, "upload", true, -5));
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Valid()
    {
        var result = _validator.TestValidate(
            new CreateVideoCommand(Guid.NewGuid(), "Title", "Desc", "upload-123", true, 5));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
