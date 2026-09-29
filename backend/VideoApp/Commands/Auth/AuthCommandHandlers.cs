using Google.Apis.Auth;
using MediatR;
using VideoApp.Common;
using VideoApp.Interfaces;
using VideoApp.Domain.Entities;

namespace VideoApp.Commands.Auth;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResult>
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtService _jwt;

    public RegisterCommandHandler(IUserRepository users, IPasswordHasher hasher, IJwtService jwt)
    {
        _users = users; _hasher = hasher; _jwt = jwt;
    }

    public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken ct)
    {
        if (await _users.GetByEmailAsync(request.Email) is not null)
            return AuthResult.Fail("Email already registered");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = _hasher.Hash(request.Password),
            FullName = request.FullName,
            CreatedAt = DateTime.UtcNow
        };

        await _users.CreateAsync(user);
        var token = _jwt.GenerateToken(user);

        return AuthResult.Ok(token, new
        {
            user.Id, user.Email, user.FullName, user.IsPremium, user.DarkMode
        });
    }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtService _jwt;

    public LoginCommandHandler(IUserRepository users, IPasswordHasher hasher, IJwtService jwt)
    {
        _users = users; _hasher = hasher; _jwt = jwt;
    }

    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _users.GetByEmailAsync(request.Email);
        if (user?.PasswordHash is null || !_hasher.Verify(request.Password, user.PasswordHash))
            return AuthResult.Fail("Invalid credentials");

        var token = _jwt.GenerateToken(user);
        return AuthResult.Ok(token, new
        {
            user.Id, user.Email, user.FullName, user.IsPremium, user.DarkMode
        });
    }
}

public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, AuthResult>
{
    private readonly IUserRepository _users;
    private readonly IJwtService _jwt;

    public GoogleLoginCommandHandler(IUserRepository users, IJwtService jwt)
    {
        _users = users; _jwt = jwt;
    }

    public async Task<AuthResult> Handle(GoogleLoginCommand request, CancellationToken ct)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);

        var user = await _users.GetByGoogleIdAsync(payload.Subject)
                   ?? await _users.GetByEmailAsync(payload.Email);

        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Email = payload.Email,
                FullName = payload.Name,
                GoogleId = payload.Subject,
                ProfilePictureUrl = payload.Picture,
                CreatedAt = DateTime.UtcNow
            };
            await _users.CreateAsync(user);
        }
        else if (user.GoogleId is null)
        {
            user.GoogleId = payload.Subject;
            await _users.UpdateAsync(user);
        }

        var token = _jwt.GenerateToken(user);
        return AuthResult.Ok(token, new
        {
            user.Id, user.Email, user.FullName, user.IsPremium, user.DarkMode
        });
    }
}
