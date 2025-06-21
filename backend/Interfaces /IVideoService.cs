namespace videohub_app.backend.Interfaces
{
    public interface IVideoService
    {
        Task<Video> UploadVideo(VideoUploadRequest request);
        Task DeleteVideo(string videoId);
        Task<Video> GetVideoDetails(string videoId);
    }
}
