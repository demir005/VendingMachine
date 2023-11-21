using VendingMachine.Models;
using VendingMachine.Models.Dto;

namespace VendingMachine.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProduct();
        Task<Product> GetProductById(int id);
        Task<Product> CreateProduct(Product product, string sellerId);
        Task<Product> UpdateProductAsync(int productId, UpdateProductRequest product, string sellerId);
        Task<bool> DeleteProduct(int id, string sellerId);


        Task<(bool success, string message, ReceiptDTO receipt)> BuyProduct(string buyerId, int productId, int amount);
    }
}
