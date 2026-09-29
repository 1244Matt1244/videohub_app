namespace VideoApp.Dtos;

public record VideoDto(
    Guid Id, string Title, string? Description, string? PlaybackId,
    string? ThumbnailUrl, bool IsPremium, decimal Price, string Status,
    DateTime CreatedAt, bool IsOwner);
