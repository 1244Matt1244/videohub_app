using MediatR;
using VideoApp.Common;

namespace VideoApp.Commands.Auth;

public record RegisterCommand(string Email, string Password, string FullName)
    : IRequest<AuthResult>;
