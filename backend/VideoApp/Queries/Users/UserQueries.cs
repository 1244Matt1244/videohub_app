using MediatR;
using VideoApp.Dtos;

namespace VideoApp.Queries.Users;

public record GetUserProfileQuery(Guid UserId) : IRequest<UserDto?>;
