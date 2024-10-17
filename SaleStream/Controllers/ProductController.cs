using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleStream.Models;
using SaleStream.Services;
using System.IO;
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

        // Vendor creates a product with image upload
        [Authorize(Roles = "Vendor")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductModel productModel, IFormFile imageFile)
        {
            var vendorEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            // Validate image file
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest(new { error = "Image file is required." });
            }

            // Save image to server
            var imageFileName = Path.GetFileNameWithoutExtension(imageFile.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine("wwwroot/images", imageFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Create new product
            var product = new Product
            {
                ProductName = productModel.ProductName,
                Price = productModel.Price,
                AvailableQuantity = productModel.AvailableQuantity,
                Category = productModel.Category,
                Description = productModel.Description,
                Image = imageFileName,  // Save image path
                VendorEmail = vendorEmail,
                StockStatus = 2,
                CategoryStatus = 1
            };

            await _productService.CreateProductAsync(product);
            return Ok(new { message = "Product created successfully." });
        }

        // Vendor updates a product with optional image upload
        [Authorize(Roles = "Vendor")]
        [HttpPut("update/{productId}")]
        public async Task<IActionResult> UpdateProduct(string productId, [FromForm] ProductModel productUpdate, IFormFile imageFile)
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

            // Handle optional image update
            if (imageFile != null && imageFile.Length > 0)
            {
                // Save new image
                var imageFileName = Path.GetFileNameWithoutExtension(imageFile.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine("wwwroot/images", imageFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Update the product's image field
                existingProduct.Image = imageFileName;
            }

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
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        // Fetch all products of a specific vendor by vendor email
        [AllowAnonymous]  // Allow everyone to access this route
        [HttpGet("vendor/{vendorEmail}")]
        public async Task<IActionResult> GetProductsByVendor(string vendorEmail)
        {
            var products = await _productService.GetProductsByVendorEmailAsync(vendorEmail);
            if (products == null || products.Count == 0)
            {
                return NotFound("No products found for this vendor.");
            }
            return Ok(products);
        }



    }
}
