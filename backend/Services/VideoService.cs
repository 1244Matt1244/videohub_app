using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public class VideoService
    {
        private readonly List<Video> _videos = new();

        public Task SaveVideoAsync(Video video)
        {
            _videos.Add(video);
            return Task.CompletedTask;
        }

        public Task<List<Video>> GetAllVideosAsync()
        {
            return Task.FromResult(_videos);
        }
    }
}
