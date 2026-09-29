using MediatR;
using VideoApp.Common;
using VideoApp.Dtos;
using VideoApp.Interfaces;
using VideoApp.Domain.Entities;

namespace VideoApp.Commands.Videos;

public class CreateUploadUrlCommandHandler : IRequestHandler<CreateUploadUrlCommand, UploadUrlResult>
{
    private readonly IMuxService _mux;
    public CreateUploadUrlCommandHandler(IMuxService mux) => _mux = mux;

    public async Task<UploadUrlResult> Handle(CreateUploadUrlCommand request, CancellationToken ct)
    {
        var result = await _mux.CreateDirectUploadAsync();
        return new UploadUrlResult(result.UploadUrl, result.UploadId);
    }
}

public class CreateVideoCommandHandler : IRequestHandler<CreateVideoCommand, ServiceResult<VideoDto>>
{
    private readonly IVideoRepository _videos;
    public CreateVideoCommandHandler(IVideoRepository videos) => _videos = videos;

    public async Task<ServiceResult<VideoDto>> Handle(CreateVideoCommand request, CancellationToken ct)
    {
        var video = new Video
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title,
            Description = request.Description,
            MuxUploadId = request.MuxUploadId,
            MuxAssetId = null,
            IsPremium = request.IsPremium,
            Price = request.Price,
            Status = "preparing",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _videos.CreateAsync(video);
        return ServiceResult<VideoDto>.Ok(new VideoDto(
            video.Id, video.Title, video.Description, null,
            null, video.IsPremium, video.Price, video.Status,
            video.CreatedAt, true));
    }
}

public class UpdateVideoCommandHandler : IRequestHandler<UpdateVideoCommand, ServiceResult<VideoDto>>
{
    private readonly IVideoRepository _videos;
    public UpdateVideoCommandHandler(IVideoRepository videos) => _videos = videos;

    public async Task<ServiceResult<VideoDto>> Handle(UpdateVideoCommand request, CancellationToken ct)
    {
        var video = await _videos.GetByIdAsync(request.Id);
        if (video is null) return ServiceResult<VideoDto>.Fail("Video not found");
        if (video.UserId != request.UserId) return ServiceResult<VideoDto>.Fail("Forbidden");

        video.Title = request.Title;
        video.Description = request.Description;
        video.IsPremium = request.IsPremium;
        video.Price = request.Price;
        video.UpdatedAt = DateTime.UtcNow;

        await _videos.UpdateAsync(video);
        return ServiceResult<VideoDto>.Ok(new VideoDto(
            video.Id, video.Title, video.Description, video.MuxPlaybackId,
            video.ThumbnailUrl, video.IsPremium, video.Price, video.Status,
            video.CreatedAt, true));
    }
}

public class DeleteVideoCommandHandler : IRequestHandler<DeleteVideoCommand, ServiceResult<bool>>
{
    private readonly IVideoRepository _videos;
    private readonly IMuxService _mux;

    public DeleteVideoCommandHandler(IVideoRepository videos, IMuxService mux)
    {
        _videos = videos; _mux = mux;
    }

    public async Task<ServiceResult<bool>> Handle(DeleteVideoCommand request, CancellationToken ct)
    {
        var video = await _videos.GetByIdAsync(request.Id);
        if (video is null) return ServiceResult<bool>.Fail("Video not found");
        if (video.UserId != request.UserId) return ServiceResult<bool>.Fail("Forbidden");

        if (!string.IsNullOrEmpty(video.MuxAssetId))
        {
            try { await _mux.DeleteAssetAsync(video.MuxAssetId); } catch { }
        }

        await _videos.DeleteAsync(video.Id);
        return ServiceResult<bool>.Ok(true);
    }
}

public class MarkVideoReadyCommandHandler : IRequestHandler<MarkVideoReadyCommand, bool>
{
    private readonly IVideoRepository _videos;
    public MarkVideoReadyCommandHandler(IVideoRepository videos) => _videos = videos;

    public async Task<bool> Handle(MarkVideoReadyCommand request, CancellationToken ct)
    {
        var video = await _videos.GetByUploadIdAsync(request.UploadId);
        if (video is null) return false;

        video.MuxAssetId = request.AssetId;
        video.MuxPlaybackId = request.PlaybackId;
        video.ThumbnailUrl = $"https://image.mux.com/{request.PlaybackId}/thumbnail.png";
        video.Status = "ready";
        video.UpdatedAt = DateTime.UtcNow;

        await _videos.UpdateAsync(video);
        return true;
    }
}

public class MarkVideoFailedCommandHandler : IRequestHandler<MarkVideoFailedCommand, bool>
{
    private readonly IVideoRepository _videos;
    public MarkVideoFailedCommandHandler(IVideoRepository videos) => _videos = videos;

    public async Task<bool> Handle(MarkVideoFailedCommand request, CancellationToken ct)
    {
        var video = await _videos.GetByAssetIdAsync(request.AssetId);
        if (video is null) return false;
        video.Status = "errored";
        await _videos.UpdateAsync(video);
        return true;
    }
}
