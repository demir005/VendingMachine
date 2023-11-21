using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VendingMachine.Controllers;
using VendingMachine.Models;
using VendingMachine.Models.Dto;
using VendingMachine.Services.Interfaces;

namespace VendingMachine.Tests
{
    public class ProductControllerTests
    {
        [Test]
        public async Task BuyProduct_ReturnsOkResult()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.BuyProduct(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((true, "Purchase successful", new ReceiptDTO()));

            var loggerMock = Mock.Of<ILogger<ProductController>>();
            var controller = new ProductController(productServiceMock.Object, loggerMock);


            var validBuyRequest = new BuyRequestDTO
            {
                ProductId = 3, 
                Amount = 123, 
            };

            // Act
            var result = await controller.BuyProduct(validBuyRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            Assert.NotNull(result);

        }

        [Test]
        public async Task GetAllProducts_ReturnsOkResult()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.GetAllProduct())
                .ReturnsAsync(new List<Product>()); 

            var loggerMock = Mock.Of<ILogger<ProductController>>();
            var controller = new ProductController(productServiceMock.Object, loggerMock);

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.NotNull(result);

            var okObjectResult = result as OkObjectResult;
            var products = okObjectResult.Value as IEnumerable<Product>;
        }
    }
}
