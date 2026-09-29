using VideoApp.Domain.Entities;

namespace VideoApp.Interfaces;

public interface IPaymentRepository
{
    Task<Guid> CreateAsync(Payment payment);
    Task UpdateStatusAsync(string sessionId, string status);
}
