using Dapper;
using VideoApp.Interfaces;
using VideoApp.Domain.Entities;

namespace VideoApp.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly IDbConnectionFactory _factory;
    public VideoRepository(IDbConnectionFactory factory) => _factory = factory;

    public async Task<IEnumerable<Video>> GetAllAsync(Guid? userId = null)
    {
        using var conn = _factory.CreateConnection();
        var sql = userId.HasValue
            ? "SELECT * FROM Videos WHERE UserId = @UserId ORDER BY CreatedAt DESC"
            : "SELECT * FROM Videos ORDER BY CreatedAt DESC";
        return await conn.QueryAsync<Video>(sql, new { UserId = userId });
    }

    public async Task<Video?> GetByIdAsync(Guid id)
    {
        using var conn = _factory.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<Video>(
            "SELECT * FROM Videos WHERE Id = @Id", new { Id = id });
    }

    public async Task<Video?> GetByUploadIdAsync(string uploadId)
    {
        using var conn = _factory.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<Video>(
            "SELECT * FROM Videos WHERE MuxUploadId = @uploadId", new { uploadId });
    }

    public async Task<Video?> GetByAssetIdAsync(string assetId)
    {
        using var conn = _factory.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<Video>(
            "SELECT * FROM Videos WHERE MuxAssetId = @assetId", new { assetId });
    }

    public async Task<Guid> CreateAsync(Video video)
    {
        using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"
            INSERT INTO Videos (Id, UserId, Title, Description, MuxUploadId, MuxAssetId, MuxPlaybackId, ThumbnailUrl, IsPremium, Price, Status, CreatedAt, UpdatedAt)
            VALUES (@Id, @UserId, @Title, @Description, @MuxUploadId, @MuxAssetId, @MuxPlaybackId, @ThumbnailUrl, @IsPremium, @Price, @Status, @CreatedAt, @UpdatedAt)",
            video);
        return video.Id;
    }

    public async Task UpdateAsync(Video video)
    {
        using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync(@"
            UPDATE Videos SET
                Title = @Title, Description = @Description,
                MuxAssetId = @MuxAssetId, MuxPlaybackId = @MuxPlaybackId,
                ThumbnailUrl = @ThumbnailUrl,
                IsPremium = @IsPremium, Price = @Price,
                Status = @Status, UpdatedAt = @UpdatedAt
            WHERE Id = @Id", video);
    }

    public async Task DeleteAsync(Guid id)
    {
        using var conn = _factory.CreateConnection();
        await conn.ExecuteAsync("DELETE FROM Videos WHERE Id = @Id", new { Id = id });
    }

    public async Task<bool> UserOwnsAsync(Guid videoId, Guid userId)
    {
        using var conn = _factory.CreateConnection();
        return await conn.ExecuteScalarAsync<bool>(
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM Videos WHERE Id=@videoId AND UserId=@userId) THEN 1 ELSE 0 END",
            new { videoId, userId });
    }
}
