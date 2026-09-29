using MediatR;
using VideoApp.Dtos;

namespace VideoApp.Queries.Videos;

public record GetVideosQuery(Guid UserId, bool OnlyMine) : IRequest<IEnumerable<VideoDto>>;
public record GetVideoByIdQuery(Guid Id, Guid UserId) : IRequest<VideoDto?>;
