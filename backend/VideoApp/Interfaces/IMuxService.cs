namespace VideoApp.Interfaces;

public interface IMuxService
{
    Task<MuxUploadResult> CreateDirectUploadAsync();
    Task<MuxAssetInfo?> GetAssetAsync(string assetId);
    Task DeleteAssetAsync(string assetId);
}

public record MuxUploadResult(string UploadUrl, string UploadId);
public record MuxAssetInfo(string AssetId, string? PlaybackId, string Status, string? ThumbnailUrl);
