using Xunit;
using Microsoft.AspNetCore.Mvc;
using VideoHubApp.backend.Controllers;
using VideoHubApp.backend.Models;

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
        public void HomeController_ReturnsOk()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UserModel_DefaultValues()
        {
            var user = new User();
            Assert.Null(user.Email);
            Assert.Null(user.Password);
        }
    }
}
