using VideoApp.Domain.Entities;

namespace VideoApp.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByGoogleIdAsync(string googleId);
    Task<Guid> CreateAsync(User user);
    Task UpdateAsync(User user);
}

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}

public interface IJwtService
{
    string GenerateToken(User user);
}
