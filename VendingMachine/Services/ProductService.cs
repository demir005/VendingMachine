using Microsoft.EntityFrameworkCore;
using VendingMachine.Data;
using VendingMachine.Models;

namespace VendingMachine.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllProduct()
        {
            return await  _dbContext.Products.Include(p=>p.SellerId).ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _dbContext.Products.Include(p => p.Seller).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Product> CreateProduct(Product product, string sellerId)
        {
            if(product.SellerId == sellerId)
            {
                throw new UnauthorizedAccessException("You do not have permission to create this product.");
            }
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }
        public async Task<Product> UpdateProductAsync(int productId, Product product, string sellerId)
        {
            var existingProduct = await _dbContext.Products.FindAsync(productId);

            if(existingProduct == null) 
            {
                return null;
            }

            if(existingProduct.SellerId == sellerId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this product.");
            }
            existingProduct.ProductName = product.ProductName;
            existingProduct.AmountAvailable = product.AmountAvailable;
            existingProduct.Cost = product.Cost;

            await _dbContext.SaveChangesAsync();
            return existingProduct;
        }
        public async Task<bool> DeleteProduct(int id, string sellerId)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if(product != null)
            {
                return false;
            }

            if(product.SellerId != sellerId)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this product.");
            }
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return true;
        }        
    }
}
