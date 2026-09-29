using MediatR;
using VideoApp.Dtos;
using VideoApp.Interfaces;

namespace VideoApp.Queries.Users;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserDto?>
{
    private readonly IUserRepository _users;
    public GetUserProfileQueryHandler(IUserRepository users) => _users = users;

    public async Task<UserDto?> Handle(GetUserProfileQuery request, CancellationToken ct)
    {
        var u = await _users.GetByIdAsync(request.UserId);
        if (u is null) return null;
        return new UserDto(u.Id, u.Email, u.FullName, u.IsPremium, u.DarkMode, u.ProfilePictureUrl);
    }
}
