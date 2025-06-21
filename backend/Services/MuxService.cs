using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VideoHubApp.Services
{
    public class MuxService
    {
        private readonly HttpClient _httpClient;

        public MuxService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.mux.com/");

            var muxTokenId = Environment.GetEnvironmentVariable("MUX_TOKEN_ID") ?? "YOUR_MUX_TOKEN_ID";
            var muxSecret = Environment.GetEnvironmentVariable("MUX_SECRET") ?? "YOUR_MUX_SECRET";

            var byteArray = Encoding.ASCII.GetBytes($"{muxTokenId}:{muxSecret}");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task<string> CreateAssetAsync(string playbackId)
        {
            var payload = new
            {
                input = new[] { $"https://stream.mux.com/{playbackId}.m3u8" },
                playback_policy = new[] { "public" }
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("video/v1/assets", content);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
