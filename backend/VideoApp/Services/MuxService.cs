using Microsoft.AspNetCore.Http;
using VideoApp.Interfaces;

namespace VideoApp.Services
{
    public class MuxService : IMuxService
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly HttpClient _httpClient;

        public MuxService(IConfiguration config, HttpClient httpClient)
        {
            _apiKey = config["Mux:Key"] ?? throw new ArgumentNullException(nameof(config));
            _apiSecret = config["Mux:Secret"] ?? throw new ArgumentNullException(nameof(config));
            _httpClient = httpClient;
        }

        public async Task<string> UploadVideo(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            return await UploadVideoAsync(stream, file.FileName);
        }

        public async Task<string> UploadVideoAsync(Stream fileStream, string fileName)
        {
            // Simulacija Mux uploada
            await Task.Delay(500);
            return $"mux_asset_{Guid.NewGuid()}";
        }

        public async Task DeleteVideo(string assetId)
        {
            // Simulacija brisanja
            await Task.Delay(200);
        }
    }
}