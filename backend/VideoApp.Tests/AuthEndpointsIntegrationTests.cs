using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Moq;
using VideoApp.Domain.Entities;
using Xunit;

namespace VideoApp.Tests;

public class AuthEndpointsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AuthEndpointsIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_With_Valid_Data_Returns_200_And_Token()
    {
        // Arrange
        _factory.UserRepositoryMock.Reset();
        _factory.UserRepositoryMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);
        _factory.UserRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u.Id);

        var body = new
        {
            email = "integration@test.com",
            password = "password123",
            fullName = "Integration Test"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/register", body);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);
        Assert.True(json.RootElement.TryGetProperty("token", out var token));
        Assert.False(string.IsNullOrEmpty(token.GetString()));
    }

    [Fact]
    public async Task Register_With_Invalid_Email_Returns_400_With_Validation_Errors()
    {
        // Arrange
        var body = new
        {
            email = "not-an-email",
            password = "password123",
            fullName = "Test"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/register", body);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Validation failed", content);
        Assert.Contains("Email", content);
    }

    [Fact]
    public async Task Register_With_Short_Password_Returns_400()
    {
        // Arrange
        var body = new
        {
            email = "valid@test.com",
            password = "short",
            fullName = "Test"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/register", body);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Password", content);
    }

    [Fact]
    public async Task Register_With_Existing_Email_Returns_400()
    {
        // Arrange
        _factory.UserRepositoryMock.Reset();
        _factory.UserRepositoryMock
            .Setup(x => x.GetByEmailAsync("existing@test.com"))
            .ReturnsAsync(new User { Id = Guid.NewGuid(), Email = "existing@test.com" });

        var body = new
        {
            email = "existing@test.com",
            password = "password123",
            fullName = "Test"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/register", body);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("already registered", content);
    }

    [Fact]
    public async Task Videos_Without_Token_Returns_401()
    {
        // Act
        var response = await _client.GetAsync("/api/Videos");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Videos_With_Invalid_Token_Returns_401()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "invalid.token.here");

        // Act
        var response = await _client.GetAsync("/api/Videos");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
