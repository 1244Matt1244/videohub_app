using Xunit;
using backend.Controllers;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VideoHubBackend.Tests
{
    // Fake MuxService
    public class FakeMuxService : MuxService
    {
        public override Task<MuxAsset> UploadVideoByUrlAsync(string url)
        {
            return Task.FromResult(new MuxAsset
            {
                Id = "123",
                PlaybackIds = new List<PlaybackId> { new PlaybackId { Id = "playback123" } }
            });
        }
    }

    // Fake VideoService
    public class FakeVideoService : VideoService
    {
        public override Task SaveVideoAsync(Video video) => Task.CompletedTask;
        public override Task<List<Video>> GetAllVideosAsync() => Task.FromResult(new List<Video>());
    }

    public class BasicTests
    {
        [Fact]
        public async Task VideoUpload_ReturnsOk()
        {
            var controller = new VideoController(new FakeMuxService(), new FakeVideoService());
            var request = new VideoUploadRequest
            {
                Title = "Test Video",
                Url = "http://example.com/video.mp4",
                Description = "Opis videa"
            };

            var result = await controller.Upload(request);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task VideoList_ReturnsOk()
        {
            var controller = new VideoController(new FakeMuxService(), new FakeVideoService());
            var result = await controller.List();
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
