using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using VideoApp.Interfaces;

namespace VideoApp.Services;

public class MuxService : IMuxService
{
    private readonly HttpClient _http;
    private readonly string _baseUrl = "https://api.mux.com";

    public MuxService(IConfiguration config)
    {
        var tokenId = config["Mux:TokenId"] ?? "";
        var tokenSecret = config["Mux:TokenSecret"] ?? "";

        var credentials = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{tokenId}:{tokenSecret}"));

        _http = new HttpClient();
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", credentials);
    }

    public async Task<MuxUploadResult> CreateDirectUploadAsync()
    {
        var payload = new
        {
            cors_origin = "*",
            new_asset_settings = new
            {
                playback_policy = new[] { "public" }
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8, "application/json");

        var response = await _http.PostAsync($"{_baseUrl}/video/v1/uploads", content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);
        var data = doc.RootElement.GetProperty("data");

        var url = data.GetProperty("url").GetString()!;
        var id = data.GetProperty("id").GetString()!;

        return new MuxUploadResult(url, id);
    }

    public async Task<MuxAssetInfo?> GetAssetAsync(string assetId)
    {
        try
        {
            var response = await _http.GetAsync($"{_baseUrl}/video/v1/assets/{assetId}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            var data = doc.RootElement.GetProperty("data");

            var id = data.GetProperty("id").GetString()!;
            var status = data.GetProperty("status").GetString()!;

            string? playbackId = null;
            string? thumbnailUrl = null;

            if (data.TryGetProperty("playback_ids", out var playbackIds)
                && playbackIds.GetArrayLength() > 0)
            {
                playbackId = playbackIds[0].GetProperty("id").GetString();
                thumbnailUrl = $"https://image.mux.com/{playbackId}/thumbnail.png";
            }

            return new MuxAssetInfo(id, playbackId, status, thumbnailUrl);
        }
        catch
        {
            return null;
        }
    }

    public async Task DeleteAssetAsync(string assetId)
    {
        await _http.DeleteAsync($"{_baseUrl}/video/v1/assets/{assetId}");
    }
}
