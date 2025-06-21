using Microsoft.Extensions.Configuration;
using Mux.Video;
using Mux.Video.Api;
using Mux.Video.Models;
using System;
using System.Threading.Tasks;

namespace backend.Services
{
    public class MuxService
    {
        private readonly AssetsApi _assetsApi;

        public MuxService(IConfiguration config)
        {
            var muxTokenId = config["Mux:TokenId"];
            var muxTokenSecret = config["Mux:TokenSecret"];

            if (string.IsNullOrEmpty(muxTokenId) || string.IsNullOrEmpty(muxTokenSecret))
                throw new ArgumentException("Mux API keys are missing");

            Configuration muxConfig = new Configuration
            {
                Username = muxTokenId,
                Password = muxTokenSecret
            };

            _assetsApi = new AssetsApi(muxConfig);
        }

        public async Task<Asset> UploadVideoAsync()
        {
            var upload = new CreateUploadRequest
            {
                NewAssetSettings = new CreateAssetRequest
                {
                    PlaybackPolicy = new System.Collections.Generic.List<string> { "public" }
                }
            };

            var uploadResult = await _assetsApi.CreateUploadAsync(upload);
            return await _assetsApi.GetAssetAsync(uploadResult.Data.AssetId);
        }
    }
}
