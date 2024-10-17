using MongoDB.Driver;
using SaleStream.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaleStream.Repositories
{
    public class ProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(IMongoClient client)
        {
            var database = client.GetDatabase("salestream");
            _products = database.GetCollection<Product>("Products");
        }

        // Create a new product in the database
        public async Task CreateProduct(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        // Retrieve all products
        public async Task<List<Product>> GetAllProducts()
        {
            return await _products.Find(_ => true).ToListAsync();
        }

        // Retrieve a product by ID
        public async Task<Product> GetProductById(string id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        // Update an existing product
        public async Task UpdateProduct(Product product)
        {
            await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
        }

        // Delete a product by its ID
        public async Task<bool> DeleteProduct(string id)
        {
            var result = await _products.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount > 0;
        }

        // Update stock status of a product
        public async Task UpdateStockStatus(string productId, int stockStatus)
        {
            var product = await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
            if (product != null)
            {
                product.StockStatus = stockStatus;
                product.LowStockStatusNotificationDateAndTime = DateTime.Now;
                await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
            }
        }

        // Update category status of products in a category
        public async Task UpdateCategoryStatus(string category, int categoryStatus)
        {
            var filter = Builders<Product>.Filter.Eq("Category", category);
            var update = Builders<Product>.Update.Set("CategoryStatus", categoryStatus);
            await _products.UpdateManyAsync(filter, update);
        }

        public async Task<List<Product>> GetProductsByVendorEmail(string vendorEmail)
        {
            return await _products.Find(p => p.VendorEmail == vendorEmail).ToListAsync();
        }

    }
}
