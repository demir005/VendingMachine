using VendingMachine.Models;

namespace VendingMachine.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProduct();
        Task<Product> GetProductById(int id);
        Task<Product> CreateProduct(Product product,int sellerId);
        Task<Product> UpdateProductAsync(int productId, Product product, int sellerId);
        Task<bool> DeleteProduct(int id, int sellerId);
    }
}
