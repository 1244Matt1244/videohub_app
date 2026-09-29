using Moq;
using Xunit;
using VideoApp.Commands.Auth;
using VideoApp.Interfaces;
using VideoApp.Domain.Entities;

namespace VideoApp.Tests;

public class AuthCommandHandlerTests
{
    [Fact]
    public async Task Register_ShouldFail_WhenEmailExists()
    {
        var users = new Mock<IUserRepository>();
        users.Setup(x => x.GetByEmailAsync("test@test.com"))
             .ReturnsAsync(new User { Id = Guid.NewGuid(), Email = "test@test.com" });

        var handler = new RegisterCommandHandler(
            users.Object,
            new Mock<IPasswordHasher>().Object,
            new Mock<IJwtService>().Object);

        var result = await handler.Handle(
            new RegisterCommand("test@test.com", "pass1234", "Test"), default);

        Assert.False(result.Success);
        Assert.Contains("already registered", result.Error);
    }

    [Fact]
    public async Task Login_ShouldFail_WithWrongPassword()
    {
        var hasher = new Mock<IPasswordHasher>();
        hasher.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        var users = new Mock<IUserRepository>();
        users.Setup(x => x.GetByEmailAsync("a@a.com"))
             .ReturnsAsync(new User
             {
                 Id = Guid.NewGuid(),
                 Email = "a@a.com",
                 PasswordHash = "hash"
             });

        var handler = new LoginCommandHandler(users.Object, hasher.Object, new Mock<IJwtService>().Object);
        var result = await handler.Handle(new LoginCommand("a@a.com", "wrong"), default);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Login_ShouldSucceed_WithValidCredentials()
    {
        var hasher = new Mock<IPasswordHasher>();
        hasher.Setup(x => x.Verify("correct", "hash")).Returns(true);

        var jwt = new Mock<IJwtService>();
        jwt.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("test-token");

        var users = new Mock<IUserRepository>();
        users.Setup(x => x.GetByEmailAsync("a@a.com"))
             .ReturnsAsync(new User
             {
                 Id = Guid.NewGuid(),
                 Email = "a@a.com",
                 PasswordHash = "hash",
                 FullName = "Test User"
             });

        var handler = new LoginCommandHandler(users.Object, hasher.Object, jwt.Object);
        var result = await handler.Handle(new LoginCommand("a@a.com", "correct"), default);

        Assert.True(result.Success);
        Assert.Equal("test-token", result.Token);
    }
}
