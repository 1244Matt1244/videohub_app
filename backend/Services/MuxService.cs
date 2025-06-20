using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace backend.Services
{
    public class MuxService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public MuxService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;

            var muxTokenId = _config["Mux:TokenId"];
            var muxTokenSecret = _config["Mux:TokenSecret"];

            var byteArray = Encoding.ASCII.GetBytes($"{muxTokenId}:{muxTokenSecret}");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task<MuxAssetDto> UploadVideoAsync()
        {
            var payload = new
            {
                input = "https://storage.googleapis.com/muxdemofiles/mux-video-intro.mp4",
                playback_policy = new[] { "public" }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.mux.com/video/v1/assets", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var parsed = JsonSerializer.Deserialize<MuxAssetResponse>(responseBody);

            return parsed?.data ?? throw new Exception("MUX API nije vratio asset.");
        }
    }

    public class MuxAssetResponse
    {
        public required MuxAssetDto data { get; set; }
    }

    public class MuxAssetDto
    {
        public required string Id { get; set; }
        public required object[] PlaybackIds { get; set; }
        public required string Status { get; set; }
    }
}
