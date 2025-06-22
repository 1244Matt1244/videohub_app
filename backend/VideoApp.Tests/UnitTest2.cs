using Xunit;
using Moq;
using VideoApp.Services;
using VideoApp.Interfaces;
using VideoApp.Controllers;
using VideoApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace VideoApp.Tests
{
    public class VideoServiceTests
    {
        [Fact]
        public async Task DeleteVideo_CallsMuxServiceWithCorrectId()
        {
            // Arrange
            var mockMux = new Mock<IMuxService>();
            var service = new VideoService(mockMux.Object);
            string testAssetId = Guid.NewGuid().ToString();

            // Act
            await service.DeleteVideo(testAssetId);

            // Assert
            mockMux.Verify(x => x.DeleteVideo(testAssetId), Times.Once);
        }

        [Fact]
        public async Task GetAllVideos_ReturnsEmptyList()
        {
            // Arrange
            var mockMux = new Mock<IMuxService>();
            var service = new VideoService(mockMux.Object);

            // Act
            var result = await service.GetAllVideos();

            // Assert
            Assert.Empty(result);
        }
    }

    public class MuxControllerTests
    {
        [Fact]
        public async Task UploadVideo_WithValidFile_ReturnsOk()
        {
            // Arrange
            var mockService = new Mock<IMuxService>();
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1024);
            fileMock.Setup(f => f.FileName).Returns("test.mp4");
            
            var controller = new MuxController(mockService.Object);

            // Act
            var result = await controller.UploadVideo(fileMock.Object);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UploadVideo_WithEmptyFile_ReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<IMuxService>();
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(0);
            
            var controller = new MuxController(mockService.Object);

            // Act
            var result = await controller.UploadVideo(fileMock.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}