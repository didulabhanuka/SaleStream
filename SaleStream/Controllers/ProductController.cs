
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleStream.Models;
using SaleStream.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SaleStream.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // Vendor creates a product
        [Authorize(Roles = "Vendor")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductModel productModel)
        {
            var vendorEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            // Create new product
            var product = new Product
            {
                ProductName = productModel.ProductName,
                Price = productModel.Price,
                AvailableQuantity = productModel.AvailableQuantity,
                Category = productModel.Category,
                Description = productModel.Description,
                VendorEmail = vendorEmail,
                StockStatus = 2,
                CategoryStatus = 1
            };

            await _productService.CreateProductAsync(product);
            return Ok(new { message = "Product created successfully." });
        }

        // Vendor updates a product
        [Authorize(Roles = "Vendor")]
        [HttpPut("update/{productId}")]
        public async Task<IActionResult> UpdateProduct(string productId, [FromBody] ProductModel productUpdate)
        {
            var existingProduct = await _productService.GetProductByIdAsync(productId);
            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }

            var vendorEmail = User.FindFirst(ClaimTypes.Email)?.Value;  // Vendor Email from token
            if (existingProduct.VendorEmail != vendorEmail)
            {
                return Unauthorized("You can only update your own products.");
            }

            // Update fields
            existingProduct.ProductName = productUpdate.ProductName;
            existingProduct.Price = productUpdate.Price;
            existingProduct.AvailableQuantity = productUpdate.AvailableQuantity;
            existingProduct.Category = productUpdate.Category;
            existingProduct.Description = productUpdate.Description;

            await _productService.UpdateProductAsync(existingProduct);
            return Ok("Product updated successfully.");
        }

        // Vendor deletes a product
        [Authorize(Roles = "Vendor")]
        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            var existingProduct = await _productService.GetProductByIdAsync(productId);
            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }

            var vendorEmail = User.FindFirst(ClaimTypes.Email)?.Value;  // Vendor Email from token
            if (existingProduct.VendorEmail != vendorEmail)
            {
                return Unauthorized("You can only delete your own products.");
            }

            await _productService.DeleteProductAsync(productId);
            return Ok("Product deleted successfully.");
        }

        // Admin updates stock status
        [Authorize(Roles = "Admin")]
        [HttpPut("stock-status/{productId}")]
        public async Task<IActionResult> UpdateStockStatus(string productId, [FromBody] StockStatusModel model)
        {
            await _productService.UpdateStockStatusAsync(productId, model.StockStatus);
            return Ok("Stock status updated successfully.");
        }

        public class StockStatusModel
        {
            public int StockStatus { get; set; }
        }

        // Admin updates category status
        [Authorize(Roles = "Admin")]
        [HttpPut("category-status")]
        public async Task<IActionResult> UpdateCategoryStatus([FromBody] CategoryStatusModel categoryStatusModel)
        {
            await _productService.UpdateCategoryStatusAsync(categoryStatusModel.Category, categoryStatusModel.CategoryStatus);
            return Ok("Category status updated successfully.");
        }

        public class CategoryStatusModel
        {
            public string Category { get; set; }
            public int CategoryStatus { get; set; }
        }

        // Fetch specific product details
        [Authorize]
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct(string productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            return Ok(product);
        }

        // Fetch all products
        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetProductsAsync();  // Fetch all products from the service
                return Ok(products);  // Return them as JSON response
            }
            catch (Exception ex)
            {
                // Log the full exception details
                Console.WriteLine($"Error fetching products: {ex}");
                return StatusCode(500, "An error occurred while fetching products.");
            }
        }
    }
}
