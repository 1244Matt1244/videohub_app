using MediatR;
using VideoApp.Common;

namespace VideoApp.Commands.Auth;

public record GoogleLoginCommand(string IdToken) : IRequest<AuthResult>;
