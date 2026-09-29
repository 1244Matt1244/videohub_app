using VideoApp.Domain.Entities;

namespace VideoApp.Interfaces;

public interface IVideoRepository
{
    Task<IEnumerable<Video>> GetAllAsync(Guid? userId = null);
    Task<Video?> GetByIdAsync(Guid id);
    Task<Video?> GetByUploadIdAsync(string uploadId);
    Task<Video?> GetByAssetIdAsync(string assetId);
    Task<Guid> CreateAsync(Video video);
    Task UpdateAsync(Video video);
    Task DeleteAsync(Guid id);
    Task<bool> UserOwnsAsync(Guid videoId, Guid userId);
}
