using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VendingMachine.Data;
using VendingMachine.Extensions;
using VendingMachine.Models;
using VendingMachine.Models.Dto;

namespace VendingMachine.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProductService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Product>> GetAllProduct()
        {
            return await  _dbContext.Products.Include(p=>p.SellerId).ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _dbContext.Products.Include(p => p.SellerId).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Product> CreateProduct(Product product, string sellerId)
        {
            if(product.SellerId != sellerId)
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


        public async Task<(bool success, string message, ReceiptDTO receipt)> BuyProduct(string buyerId, int productId, int amount)
        {
            var buyer = await _dbContext.ApplicationUsers.FindAsync(buyerId);
            var product = await _dbContext.Products.FindAsync(productId);

            if (buyer == null || product == null)
            {
                return (false, "Invalid buyer or product", null);
            }

            if (buyer.Deposit < product.Cost * amount)
            {
                return (false, "Not enough funds", null);
            }

            if (product.AmountAvailable < amount)
            {
                return (false, "Not enough stock", null);
            }

            int totalCost = product.Cost * amount;
            buyer.Deposit -= totalCost;
            product.AmountAvailable -= amount;
            await _dbContext.SaveChangesAsync();

            var receipt = new ReceiptDTO
            {
                TotalSpent = totalCost,
                ProductPurchased = product.ProductName,
                Change = CalculateChange(totalCost),
            };

            return (true, "Purchase successful", receipt);
        }

        #region Private Metod
        private Dictionary<int, int> CalculateChange(int amount)
        {
            // Assuming coin values are 100, 50, 20, 10, and 5
            var coinValues = new List<int> { 100, 50, 20, 10, 5 };
            var change = new Dictionary<int, int>();

            foreach (var coinValue in coinValues)
            {
                int coinCount = amount / coinValue;
                if (coinCount > 0)
                {
                    change.Add(coinValue, coinCount);
                    amount -= coinValue * coinCount;
                }
            }

            return change;
        }
        #endregion
    }
}
