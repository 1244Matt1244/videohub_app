namespace VideoApp.Models
{
    public class VideoUploadRequest
    {
        public IFormFile VideoFile { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsPremium { get; set; }
    }
}