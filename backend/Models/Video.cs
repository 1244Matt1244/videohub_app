#nullable enable

namespace backend.Models
{
    public class Video
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string PlaybackUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
