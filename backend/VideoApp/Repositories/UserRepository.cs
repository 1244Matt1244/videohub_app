using Dapper;
using VideoApp.Interfaces;
using VideoApp.Domain.Entities;

namespace VideoApp.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _factory;
    public UserRepository(IDbConnectionFactory factory) => _factory = factory;

    public async Task<User?> GetByIdAsync(Guid id)
    {
        using var conn = _factory.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Id = @Id", new { Id = id });
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var conn = _factory.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Email = @Email", new { Email = email });
    }

    public async Task<User?> GetByGoogleIdAsync(string googleId)
    {
        using var conn = _factory.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE GoogleId = @GoogleId", new { GoogleId = googleId });
    }

    public async Task<Guid> CreateAsync(User user)
    {
        using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"
            INSERT INTO Users (Id, Email, PasswordHash, FullName, GoogleId, ProfilePictureUrl, IsPremium, DarkMode, CreatedAt)
            VALUES (@Id, @Email, @PasswordHash, @FullName, @GoogleId, @ProfilePictureUrl, @IsPremium, @DarkMode, @CreatedAt)",
            user);
        return user.Id;
    }

    public async Task UpdateAsync(User user)
    {
        using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"
            UPDATE Users SET
                Email = @Email, PasswordHash = @PasswordHash, FullName = @FullName,
                GoogleId = @GoogleId, ProfilePictureUrl = @ProfilePictureUrl,
                IsPremium = @IsPremium, PremiumExpiresAt = @PremiumExpiresAt, DarkMode = @DarkMode
            WHERE Id = @Id", user);
    }
}
