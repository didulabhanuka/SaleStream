using MongoDB.Driver;
using SaleStream.Models;

namespace SaleStream.Repositories
{

    /// Handles data access for product management.
    public class ProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(IMongoClient client)
        {
            var database = client.GetDatabase("salestream");
            _products = database.GetCollection<Product>("Products");
        }

    
        /// Creates a new product in the database.
        public async Task CreateProduct(Product product)
        {
            await _products.InsertOneAsync(product);
        }

    
        /// Retrieves all products (active or deactivated).
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _products.Find(_ => true).ToListAsync();
        }

    
        /// Retrieves products based on their activation status.
        public async Task<IEnumerable<Product>> GetProductsByStatus(bool isActive)
        {
            return await _products.Find(p => p.IsActive == isActive).ToListAsync();
        }

    
        /// Updates an existing product in the database.
        public async Task UpdateProduct(Product product)
        {
            await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
        }

    
        /// Deletes a product by ID from the database.
        public async Task<bool> DeleteProduct(string id)
        {
            var result = await _products.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount > 0;
        }

    
        /// Retrieves a product by ID from the database.
        public async Task<Product> GetProductById(string id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
    }
}
