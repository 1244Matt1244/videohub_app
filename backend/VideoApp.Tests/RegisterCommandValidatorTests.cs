using FluentValidation.TestHelper;
using VideoApp.Commands.Auth;
using Xunit;

namespace VideoApp.Tests;

public class RegisterCommandValidatorTests
{
    private readonly RegisterCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var result = _validator.TestValidate(new RegisterCommand("", "password123", "Name"));
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var result = _validator.TestValidate(new RegisterCommand("not-an-email", "password123", "Name"));
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        var result = _validator.TestValidate(new RegisterCommand("a@a.com", "short", "Name"));
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_FullName_Is_Empty()
    {
        var result = _validator.TestValidate(new RegisterCommand("a@a.com", "password123", ""));
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Request_Is_Valid()
    {
        var result = _validator.TestValidate(
            new RegisterCommand("test@test.com", "password123", "Test User"));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
