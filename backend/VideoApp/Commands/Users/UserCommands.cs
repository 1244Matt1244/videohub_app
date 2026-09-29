using MediatR;
using VideoApp.Common;
using VideoApp.Dtos;

namespace VideoApp.Commands.Users;

public record UpdateProfileCommand(Guid UserId, string FullName)
    : IRequest<ServiceResult<UserDto>>;

public record UpdateDarkModeCommand(Guid UserId, bool IsDark) : IRequest<bool>;
