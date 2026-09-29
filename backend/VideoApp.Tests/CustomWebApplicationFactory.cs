using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VideoApp.Interfaces;

namespace VideoApp.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IUserRepository> UserRepositoryMock { get; } = new();
    public Mock<IVideoRepository> VideoRepositoryMock { get; } = new();
    public Mock<IPaymentRepository> PaymentRepositoryMock { get; } = new();
    public Mock<IMuxService> MuxServiceMock { get; } = new();
    public Mock<IStripeService> StripeServiceMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Ukloni postojeće registracije
            var descriptorsToRemove = services
                .Where(d =>
                    d.ServiceType == typeof(IUserRepository) ||
                    d.ServiceType == typeof(IVideoRepository) ||
                    d.ServiceType == typeof(IPaymentRepository) ||
                    d.ServiceType == typeof(IMuxService) ||
                    d.ServiceType == typeof(IStripeService))
                .ToList();

            foreach (var d in descriptorsToRemove)
                services.Remove(d);

            // Registriraj mockove
            services.AddSingleton(UserRepositoryMock.Object);
            services.AddSingleton(VideoRepositoryMock.Object);
            services.AddSingleton(PaymentRepositoryMock.Object);
            services.AddSingleton(MuxServiceMock.Object);
            services.AddSingleton(StripeServiceMock.Object);
        });
    }
}
