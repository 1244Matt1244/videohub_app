using FluentValidation.TestHelper;
using VideoApp.Commands.Auth;
using Xunit;

namespace VideoApp.Tests;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Email_Empty()
    {
        var result = _validator.TestValidate(new LoginCommand("", "pass"));
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Empty()
    {
        var result = _validator.TestValidate(new LoginCommand("a@a.com", ""));
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Valid()
    {
        var result = _validator.TestValidate(new LoginCommand("a@a.com", "password"));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
