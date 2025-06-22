// IVideoService.cs
using VideoApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VideoApp.Interfaces
{
    public interface IVideoService
    {
        Task UploadVideo(Video video);
        Task DeleteVideo(string assetId);
        Task<IEnumerable<Video>> GetAllVideos();
        
        // Dodajte novu metodu
        Task UnlockPremiumVideo(string videoId);
    }
}