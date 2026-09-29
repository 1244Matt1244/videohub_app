using MediatR;
using VideoApp.Common;

namespace VideoApp.Commands.Auth;

public record LoginCommand(string Email, string Password) : IRequest<AuthResult>;
