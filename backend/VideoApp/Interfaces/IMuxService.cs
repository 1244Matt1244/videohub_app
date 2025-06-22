namespace VideoApp.Interfaces
{
    public interface IMuxService
    {
        Task<string> UploadVideo(IFormFile file);
        Task DeleteVideo(string assetId);
        Task<string> UploadVideoAsync(Stream fileStream, string fileName);
    }
}