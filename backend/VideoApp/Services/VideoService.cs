using VideoApp.Interfaces;
using VideoApp.Models;

namespace VideoApp.Services
{
    public class VideoService : IVideoService
    {
        private readonly IMuxService _muxService;

        public VideoService(IMuxService muxService)
        {
            _muxService = muxService;
        }

        public async Task UploadVideo(Video video)
        {
            if (video == null)
                throw new ArgumentNullException(nameof(video), "Video objekt ne smije biti null");

            // Ovdje dodajte stvarnu implementaciju ako je potrebno
            await Task.Delay(100);
        }

        public async Task DeleteVideo(string assetId)
        {
            if (string.IsNullOrWhiteSpace(assetId))
                throw new ArgumentException("Nevažeći ID resursa");

            await _muxService.DeleteVideo(assetId);
            await Task.Delay(100);
        }

        public async Task<IEnumerable<Video>> GetAllVideos()
        {
            await Task.Delay(100);
            return new List<Video>();
        }

        public async Task UnlockPremiumVideo(string videoId)
        {
            // Ovdje možete otključati pristup premium videu u bazi podataka
            await Task.Delay(100);
        }
    }
}
