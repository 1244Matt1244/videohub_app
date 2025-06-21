#nullable enable
namespace videohub_app.backend.Models
{
    public class Video
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string PlaybackUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsPremium { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = string.Empty;
    }
}
