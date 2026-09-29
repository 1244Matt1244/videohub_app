using MediatR;

namespace VideoApp.Commands.Videos;

public record MarkVideoReadyCommand(string UploadId, string AssetId, string PlaybackId) : IRequest<bool>;
public record MarkVideoFailedCommand(string AssetId) : IRequest<bool>;
