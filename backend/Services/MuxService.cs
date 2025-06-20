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

        public MuxService(IConfiguration config, HttpClient httpClient)
        {
            _httpClient = httpClient;

            var tokenId = config["Mux:TokenId"];
            var tokenSecret = config["Mux:TokenSecret"];

            if (string.IsNullOrEmpty(tokenId) || string.IsNullOrEmpty(tokenSecret))
            {
                throw new ArgumentNullException("Mux credentials not found in configuration");
            }

            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{tokenId}:{tokenSecret}"));
            _httpClient.BaseAddress = new Uri("https://api.mux.com");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        }

        public async Task<string> UploadVideoAsync(string playbackPolicy = "public")
        {
            var requestContent = new
            {
                input = new[] { "https://stream.mux.com/demo-video.mp4" },
                playback_policy = new[] { playbackPolicy }
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestContent),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("/video/v1/assets", jsonContent);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return responseJson; // ili deserializiraj u neki model
        }
    }
}
