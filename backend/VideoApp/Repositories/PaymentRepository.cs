using Dapper;
using VideoApp.Interfaces;
using VideoApp.Domain.Entities;

namespace VideoApp.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly IDbConnectionFactory _factory;
    public PaymentRepository(IDbConnectionFactory factory) => _factory = factory;

    public async Task<Guid> CreateAsync(Payment p)
    {
        using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"
            INSERT INTO Payments (Id, UserId, VideoId, StripeSessionId, Amount, Status, CreatedAt)
            VALUES (@Id, @UserId, @VideoId, @StripeSessionId, @Amount, @Status, @CreatedAt)", p);
        return p.Id;
    }

    public async Task UpdateStatusAsync(string sessionId, string status)
    {
        using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(
            "UPDATE Payments SET Status=@status WHERE StripeSessionId=@sessionId",
            new { status, sessionId });
    }
}
