using MediatR;
using VideoApp.Dtos;
using VideoApp.Interfaces;

namespace VideoApp.Queries.Videos;

public class GetVideosQueryHandler : IRequestHandler<GetVideosQuery, IEnumerable<VideoDto>>
{
    private readonly IVideoRepository _videos;
    private readonly IUserRepository _users;

    public GetVideosQueryHandler(IVideoRepository videos, IUserRepository users)
    {
        _videos = videos; _users = users;
    }

    public async Task<IEnumerable<VideoDto>> Handle(GetVideosQuery request, CancellationToken ct)
    {
        var user = await _users.GetByIdAsync(request.UserId);
        var isPremium = user?.IsPremium ?? false;

        var videos = await _videos.GetAllAsync(request.OnlyMine ? request.UserId : null);

        return videos.Select(v => new VideoDto(
            v.Id, v.Title, v.Description,
            v.IsPremium && !isPremium && v.UserId != request.UserId ? null : v.MuxPlaybackId,
            v.ThumbnailUrl, v.IsPremium, v.Price, v.Status, v.CreatedAt,
            v.UserId == request.UserId));
    }
}

public class GetVideoByIdQueryHandler : IRequestHandler<GetVideoByIdQuery, VideoDto?>
{
    private readonly IVideoRepository _videos;
    public GetVideoByIdQueryHandler(IVideoRepository videos) => _videos = videos;

    public async Task<VideoDto?> Handle(GetVideoByIdQuery request, CancellationToken ct)
    {
        var v = await _videos.GetByIdAsync(request.Id);
        if (v is null) return null;
        return new VideoDto(v.Id, v.Title, v.Description, v.MuxPlaybackId,
            v.ThumbnailUrl, v.IsPremium, v.Price, v.Status, v.CreatedAt,
            v.UserId == request.UserId);
    }
}
