using Microsoft.Extensions.Configuration;
using Mux.Api;
using Mux.Model;
using System;
using System.Threading.Tasks;

namespace backend.Services
{
    public class MuxService
    {
        private readonly AssetsApi _assetsApi;

        public MuxService(IConfiguration config)
        {
            var tokenId = config["Mux:TokenId"];
            var tokenSecret = config["Mux:TokenSecret"];

            if (string.IsNullOrEmpty(tokenId) || string.IsNullOrEmpty(tokenSecret))
            {
                throw new ArgumentNullException("Mux credentials not found in configuration");
            }

            Configuration muxConfig = new Configuration
            {
                Username = tokenId,
                Password = tokenSecret
            };

            _assetsApi = new AssetsApi(muxConfig);
        }

        public async Task<Asset> UploadVideoAsync(string playbackPolicy = "public")
        {
            var input = new CreateAssetRequest(
                new System.Collections.Generic.List<string> { "https://stream.mux.com/demo-video.mp4" }, // Demo input URL
                new System.Collections.Generic.List<string> { playbackPolicy }
            );

            var assetResponse = await _assetsApi.CreateAssetAsync(input);
            return assetResponse.Data;
        }
    }
}
