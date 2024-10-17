using MongoDB.Driver;
using SaleStream.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaleStream.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _products;

        public ProductService(IMongoClient client)
        {
            var database = client.GetDatabase("salestream");
            _products = database.GetCollection<Product>("Products");
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _products.Find(product => true).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            return await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
        }

        public async Task CreateProductAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
        }

        public async Task DeleteProductAsync(string productId)
        {
            await _products.DeleteOneAsync(p => p.Id == productId);
        }

        public async Task UpdateStockStatusAsync(string productId, int stockStatus)
        {
            var product = await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
            if (product != null)
            {
                product.StockStatus = stockStatus;
                product.LowStockStatusNotificationDateAndTime = DateTime.Now;

                await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
            }
        }

        public async Task UpdateCategoryStatusAsync(string category, int categoryStatus)
        {
            var filter = Builders<Product>.Filter.Eq("Category", category);
            var update = Builders<Product>.Update.Set("CategoryStatus", categoryStatus);
            await _products.UpdateManyAsync(filter, update);
        }

        public async Task<List<Product>> GetProductsByVendorEmailAsync(string vendorEmail)
        {
            return await _products.Find(product => product.VendorEmail == vendorEmail).ToListAsync();
        }


    }
}
