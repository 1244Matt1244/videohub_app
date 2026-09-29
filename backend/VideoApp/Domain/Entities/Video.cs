namespace VideoApp.Domain.Entities;

public class Video
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? MuxUploadId { get; set; }
    public string? MuxAssetId { get; set; }
    public string? MuxPlaybackId { get; set; }
    public string? ThumbnailUrl { get; set; }
    public bool IsPremium { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; } = "preparing";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
