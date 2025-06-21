using Xunit;
using backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using backend.Models;

namespace VideoHubBackend.Tests
{
    public class BasicTests
    {
        [Fact]
        public void Test1_Passes()
        {
            Assert.True(1 + 1 == 2);
        }

        [Fact]
        public void VideoUpload_ReturnsOk()
        {
            var controller = new VideoController();
            var request = new VideoUploadRequest
            {
                Title = "Test Video",
                Url = "http://example.com/video.mp4",
                Description = "Opis videa"
            };

            var result = controller.Upload(request);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void VideoList_ReturnsOk()
        {
            var controller = new VideoController();
            var result = controller.List();
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
