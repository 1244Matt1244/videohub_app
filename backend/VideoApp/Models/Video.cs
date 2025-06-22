namespace VideoApp.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string MuxAssetId { get; set; } = null!;
        public string PlaybackId { get; set; } = null!;
        public bool IsPremium { get; set; }
    }
}