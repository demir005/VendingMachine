using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VendingMachine.Models;
using VendingMachine.Models.Dto;
using VendingMachine.Services;

namespace VendingMachine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProduct();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            try
            {
                // Assuming you have the SellerId available in the authentication token
                var sellerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var createdProduct = await _productService.CreateProduct(product, sellerId);

                return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            try
            {
                // Assuming you have the SellerId available in the authentication token
                var sellerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var updatedProduct = await _productService.UpdateProductAsync(id, product, sellerId);

                if (updatedProduct == null)
                {
                    return NotFound();
                }

                return Ok(updatedProduct);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                // Assuming you have the SellerId available in the authentication token
                var sellerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _productService.DeleteProduct(id, sellerId);

                if (!result)
                {
                    return NotFound();
                }

                return Ok("Product deleted successfully");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("buy")]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> BuyProduct([FromBody] BuyRequestDTO buyModel)
        {
            try
            {
                if (buyModel == null || buyModel.ProductId <= 0 || buyModel.Amount <= 0)
                {
                    return BadRequest("Invalid request");
                }

                var buyerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var (success, message, receipt) = await _productService.BuyProduct(buyerId, buyModel.ProductId, buyModel.Amount);

                if (!success)
                {
                    return BadRequest(message);
                }

                return Ok(receipt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
