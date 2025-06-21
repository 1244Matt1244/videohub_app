#nullable enable
namespace videohub_app.backend.Models
{
    public class VideoUploadRequest
    {
        public string Title { get; set; } = string.Empty;
        public IFormFile VideoFile { get; set; } = null!; // Obavezno za upload
        public bool IsPremium { get; set; }
        public string? Description { get; set; } // Opcionalno
    }
}
