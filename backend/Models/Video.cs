namespace backend.Models
{
    public class Video
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required string PlaybackUrl { get; set; }
    }
}
