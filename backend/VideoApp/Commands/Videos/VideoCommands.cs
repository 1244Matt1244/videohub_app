using MediatR;
using VideoApp.Common;
using VideoApp.Dtos;

namespace VideoApp.Commands.Videos;

public record CreateUploadUrlCommand(Guid UserId) : IRequest<UploadUrlResult>;

public record CreateVideoCommand(
    Guid UserId, string Title, string? Description,
    string MuxUploadId, bool IsPremium, decimal Price)
    : IRequest<ServiceResult<VideoDto>>;

public record UpdateVideoCommand(
    Guid Id, Guid UserId, string Title, string? Description,
    bool IsPremium, decimal Price)
    : IRequest<ServiceResult<VideoDto>>;

public record DeleteVideoCommand(Guid Id, Guid UserId) : IRequest<ServiceResult<bool>>;

public record UploadUrlResult(string UploadUrl, string UploadId);
