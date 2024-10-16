

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
            // Initialize the MongoDB collection to work with 'Products' collection in the 'salestream' database
            var database = client.GetDatabase("salestream");
            _products = database.GetCollection<Product>("Products");
        }

        // Fetch all products from the database
        public async Task<List<Product>> GetProductsAsync()
        {
            // Use MongoDB's Find method to retrieve all products where the filter is always true (i.e., no filtering)
            return await _products.Find(product => true).ToListAsync();
        }

        // Fetch a specific product by its unique ID
        public async Task<Product> GetProductByIdAsync(string productId)
        {
            return await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
        }

        // Insert a new product into the database
        public async Task CreateProductAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        // Update an existing product in the database
        public async Task UpdateProductAsync(Product product)
        {
            // Replace the existing product with the same ID
            await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
        }

        // Delete a product by its ID
        public async Task DeleteProductAsync(string productId)
        {
            await _products.DeleteOneAsync(p => p.Id == productId);
        }

        // Update the stock status of a product
        public async Task UpdateStockStatusAsync(string productId, int stockStatus)
        {
            var product = await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
            if (product != null)
            {
                // Update stock status and record the time of update
                product.StockStatus = stockStatus;
                product.LowStockStatusNotificationDateAndTime = DateTime.Now;

                await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
            }
        }

        // Update the category status of multiple products in the same category
        public async Task UpdateCategoryStatusAsync(string category, int categoryStatus)
        {
            // Filter to match products in the specified category
            var filter = Builders<Product>.Filter.Eq("Category", category);
            // Update the CategoryStatus field for all matching products
            var update = Builders<Product>.Update.Set("CategoryStatus", categoryStatus);
            await _products.UpdateManyAsync(filter, update);
        }
    }
}
