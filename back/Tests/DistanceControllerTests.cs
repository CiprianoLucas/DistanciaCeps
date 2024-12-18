using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Back.App.HTTP;
using Back.Domain.Entities;
using Back.App.DTOs;
using Back.App;

namespace Back.Tests
{
    public class DistanceControllerTests
    {
        private readonly Mock<ILogger<DistanceController>> _mockLogger;
        private readonly Mock<Container> _mockContainer;
        private readonly DistanceController _controller;

        public DistanceControllerTests()
        {
            _mockLogger = new Mock<ILogger<DistanceController>>();
            _mockContainer = new Mock<Container>();

            _controller = new DistanceController(_mockLogger.Object);
        }

        [Fact]
        public async Task CalculateAsync_ReturnsOk_WhenValidData()
        {
            var dto = new DistanceDto { De = "89045-500", Para = "48045400" };
            var user = new User { Id = 1, Username = "TestUser", Email = "test@test.com", PasswordHash = "cb9KxVixo+n+/8Aw5d44Tw==:Gbd/0bfkPWfBt1nR1MC3SVD5rxbCJdx6EOESWe2B7CQ=" };
            var distance = new Distance { De = "89045-500", Para = "48045400", Distancia = 100 };

            _mockContainer.Setup(x => x.TokenService.VerifyToken(It.IsAny<string>())).ReturnsAsync(user);
            _mockContainer.Setup(x => x.DistanceService.GetCalculateAndSaveAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>()))
                          .ReturnsAsync((distance, true));

            var result = await _controller.CalculateAsync(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<DistanceResponseDto>(okResult.Value);
            Assert.Equal(100, response.Distancia);
        }
    }
}
