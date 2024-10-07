using Microsoft.AspNetCore.Mvc;
using SaleStream.Models;
using SaleStream.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SaleStream.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly VendorService _vendorService;
        private readonly UserService _userService;
        private readonly JwtService _jwtService;

        public VendorController(VendorService vendorService, JwtService jwtService, UserService userService)
        {
            _vendorService = vendorService;
            _jwtService = jwtService;
            _userService = userService;
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateVendor([FromBody] VendorCreateModel vendorModel)
        {
            if (await _vendorService.GetVendorByEmailAsync(vendorModel.Email) != null || await _userService.GetUserByEmailAsync(vendorModel.Email) != null)
            {
                return BadRequest("Vendor with this email already exists.");
            }

            var newVendor = new Vendor
            {
                VendorName = vendorModel.VendorName,
                Email = vendorModel.Email,
                Password = UserService.EncryptPassword(vendorModel.Password),
                Category = vendorModel.Category
            };

            await _vendorService.CreateVendorAsync(newVendor);
            return Ok("Vendor created successfully.");
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("update/{vendorId}")]
        public async Task<IActionResult> UpdateVendor(string vendorId, [FromBody] VendorCreateModel updatedVendor)
        {
            var existingVendor = await _vendorService.GetVendorByIdAsync(vendorId);
            if (existingVendor == null)
            {
                return NotFound("Vendor not found.");
            }

            existingVendor.VendorName = updatedVendor.VendorName;
            existingVendor.Email = updatedVendor.Email;
            existingVendor.Password = UserService.EncryptPassword(updatedVendor.Password);
            existingVendor.Category = updatedVendor.Category;

            await _vendorService.UpdateVendorAsync(existingVendor);
            return Ok("Vendor updated successfully.");
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("delete/{email}")]
        public async Task<IActionResult> DeleteVendor(string email)
        {
            await _vendorService.DeleteVendorAsync(email);
            return Ok("Vendor deleted successfully.");
        }

        [Authorize(Policy = "VendorPolicy, AdminPolicy")]
        [HttpPost("deactivate/{email}")]
        public async Task<IActionResult> DeactivateVendor(string email)
        {
            var vendor = await _vendorService.GetVendorByEmailAsync(email);
            if (vendor == null)
            {
                return NotFound("Vendor not found.");
            }

            vendor.Status = 0;
            await _vendorService.UpdateVendorAsync(vendor);
            return Ok("Vendor deactivated.");
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("activate/{email}")]
        public async Task<IActionResult> ActivateVendor(string email)
        {
            var vendor = await _vendorService.GetVendorByEmailAsync(email);
            if (vendor == null)
            {
                return NotFound("Vendor not found.");
            }

            vendor.Status = 1;
            await _vendorService.UpdateVendorAsync(vendor);
            return Ok("Vendor activated.");
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListVendors()
        {
            var vendors = await _vendorService.GetVendorsAsync();
            return Ok(vendors);
        }

        [HttpGet("{vendorId}")]
        public async Task<IActionResult> GetVendorById(string vendorId)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(vendorId);
            if (vendor == null)
            {
                return NotFound("Vendor not found.");
            }

            return Ok(vendor);
        }

        [Authorize]
        [HttpPost("comment/{vendorId}")]
        public async Task<IActionResult> AddComment(string vendorId, [FromBody] CommentModel model)
        {
            var userId = User.FindFirst("UserId")?.Value;
            await _vendorService.AddCommentAsync(vendorId, model.Comment, model.Rank, userId);
            return Ok("Comment added successfully.");
        }

        [Authorize]
        [HttpPut("comment/{vendorId}/{commentId}")]
        public async Task<IActionResult> UpdateComment(string vendorId, string commentId, [FromBody] CommentModel model)
        {
            var userId = User.FindFirst("UserId")?.Value;
            await _vendorService.UpdateCommentAsync(vendorId, commentId, model.Comment, model.Rank, userId);
            return Ok("Comment updated successfully.");
        }

        [Authorize]
        [HttpDelete("comment/{vendorId}/{commentId}")]
        public async Task<IActionResult> DeleteComment(string vendorId, string commentId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            await _vendorService.DeleteCommentAsync(vendorId, commentId, userId);
            return Ok("Comment deleted successfully.");
        }
    }

    public class CommentModel
    {
        public string Comment { get; set; }
        public int Rank { get; set; }
    }
}
