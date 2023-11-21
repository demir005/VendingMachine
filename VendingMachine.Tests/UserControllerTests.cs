using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Controllers;
using VendingMachine.Models;
using VendingMachine.Models.Dto;
using VendingMachine.Services.Interfaces;

namespace VendingMachine.Tests
{
    public class UserControllerTests
    {
        [Test]
        public async Task DepositCoin_ReturnsOkResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserServices>();
            userServiceMock.Setup(x => x.DepositCoin(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            var loggerMock = Mock.Of<ILogger<UserController>>();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "2c823779-2845-413a-a525-45dec7a64a50"), // replace "123" with the expected user ID
                new Claim(ClaimTypes.NameIdentifier, "55b5481f-21ea-405f-aae0-d5cba67c2a22"), // replace "123" with the expected user ID
                new Claim(ClaimTypes.NameIdentifier, "8d4b72e2-1632-42e5-b187-0e3c7a64372f"), // replace "123" with the expected user ID
                new Claim(ClaimTypes.NameIdentifier, "d7205baa-0ba5-4082-afd8-998630e96fe1") // replace "123" with the expected user ID
            };

            var identity = new ClaimsIdentity(claims, "TestAuthentication");
            var principal = new ClaimsPrincipal(identity);

            var controller = new UserController(userServiceMock.Object, loggerMock)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };


            var deposit = new DepositRequestDTO
            {
                CoinValue = 100,
            };

            // Act
            var result = await controller.DepositCoin(deposit);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.NotNull(result);
        }
    }
}
