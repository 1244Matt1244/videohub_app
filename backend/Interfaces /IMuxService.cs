namespace videohub_app.backend.Interfaces
{
    public interface IMuxService
    {
        Task<string> UploadVideo(IFormFile file);
        Task DeleteVideo(string assetId);
    }
}
