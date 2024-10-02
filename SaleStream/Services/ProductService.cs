using SaleStream.Models;
using SaleStream.Repositories;

namespace SaleStream.Services
{

    /// Handles business logic for managing products.
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        // Fixed categories for products
        private readonly List<string> _fixedCategories = new List<string>
        {
            "Electronics",
            "Books",
            "Clothing",
            "Home Appliances",
            "Furniture"
        };

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

    
        /// Returns the list of fixed categories.
        public List<string> GetFixedCategories()
        {
            return _fixedCategories;
        }


        /// Retrieves all products.
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }


        /// Retrieves all deactivated products.
        public async Task<IEnumerable<Product>> GetAllDeactivatedProducts()
        {
            return await _productRepository.GetProductsByStatus(false);  // false means deactivated
        }

    
        /// Creates a new product after validating the categories.
        public async Task<Product> CreateProduct(Product product)
        {
            // Ensure the category is valid
            if (!ValidateCategories(product.CategoryId))
            {
                throw new Exception("Invalid category selected.");
            }

            // Ensure VendorId is not null
            if (string.IsNullOrEmpty(product.VendorId))
            {
                throw new Exception("Vendor ID is required.");
            }

            // Pass the product to the repository for insertion
            await _productRepository.CreateProduct(product);
            return product;
        }


    
        /// Updates an existing product after validating the categories.
        public async Task<Product> UpdateProduct(string id, Product updatedProduct)
        {
            if (!ValidateCategories(updatedProduct.CategoryId))
            {
                throw new Exception("Invalid category selected.");
            }

            var product = await _productRepository.GetProductById(id);
            if (product == null) return null;

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.CategoryId = updatedProduct.CategoryId;
            product.Quantity = updatedProduct.Quantity;

            await _productRepository.UpdateProduct(product);
            return product;
        }

    
        /// Validates that the selected categories are valid.
        private bool ValidateCategories(string categoryId)
        {
            return _fixedCategories.Contains(categoryId);
        }


    
        /// Deletes a product by ID.
        public async Task<bool> DeleteProduct(string id)
        {
            return await _productRepository.DeleteProduct(id);
        }

    
        /// Activates a product listing.
        public async Task<Product> ActivateProduct(string id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null) return null;

            product.IsActive = true;
            await _productRepository.UpdateProduct(product);
            return product;
        }

    
        /// Deactivates a product listing.
        public async Task<Product> DeactivateProduct(string id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null) return null;

            product.IsActive = false;
            await _productRepository.UpdateProduct(product);
            return product;
        }

    
        /// Retrieves a product by ID.
        public async Task<Product> GetProductById(string id)
        {
            return await _productRepository.GetProductById(id);
        }
    }
}
