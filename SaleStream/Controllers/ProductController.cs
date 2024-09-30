using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleStream.Models;
using SaleStream.Services;

namespace SaleStream.Controllers
{

    /// Handles product management for vendors, CSRs, and administrators.
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

    
        /// Gets the list of fixed categories that everyone can access.
        [HttpGet("categories")]
        public IActionResult GetFixedCategories()
        {
            var categories = _productService.GetFixedCategories();
            return Ok(categories);
        }

    
        /// Gets all products, accessible to everyone.
        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

    
        /// Gets a specific product by its ID, accessible to everyone.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return NotFound("Product not found.");
            return Ok(product);
        }

    
        /// Gets all deactivated products, accessible to Admins, CSRs, and Vendors.
        [HttpGet("deactivated")]
        [Authorize(Policy = "VendorPolicy")]  // Only vendors can update products
        public async Task<IActionResult> GetAllDeactivatedProducts()
        {
            var deactivatedProducts = await _productService.GetAllDeactivatedProducts();
            return Ok(deactivatedProducts);
        }

    
        /// Creates a new product. Accessible only to Vendors.
        [HttpPost("create")]
        [Authorize(Policy = "VendorPolicy")]  // Only vendors can create products
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            try
            {
                var createdProduct = await _productService.CreateProduct(product);
                return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    
        /// Updates an existing product. Accessible only to Vendors.
        [HttpPut("update/{id}")]
        [Authorize(Policy = "VendorPolicy")]  // Only vendors can update products
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] Product updatedProduct)
        {
            try
            {
                var product = await _productService.UpdateProduct(id, updatedProduct);
                if (product == null)
                    return NotFound("Product not found.");
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    
        /// Deletes a product by its ID. Accessible only to Vendors.
        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "VendorPolicy")]  // Only vendors can delete products
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var isDeleted = await _productService.DeleteProduct(id);
            if (!isDeleted)
                return NotFound("Product not found.");
            return Ok("Product deleted successfully.");
        }

    
        /// Activates a product listing. Accessible only to Admins.
        [HttpPut("activate/{id}")]
        [Authorize(Policy = "VendorPolicy")]  // Only vendors can update products
        public async Task<IActionResult> ActivateProduct(string id)
        {
            var product = await _productService.ActivateProduct(id);
            if (product == null)
                return NotFound("Product not found.");
            return Ok("Product activated successfully.");
        }

    
        /// Deactivates a product listing. Accessible only to Admins.
        [HttpPut("deactivate/{id}")]
        [Authorize(Policy = "VendorPolicy")]  // Only vendors can update products
        public async Task<IActionResult> DeactivateProduct(string id)
        {
            var product = await _productService.DeactivateProduct(id);
            if (product == null)
                return NotFound("Product not found.");
            return Ok("Product deactivated successfully.");
        }
    }
}
