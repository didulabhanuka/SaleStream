using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleStream.Services;

namespace SaleStream.Controllers
{

    /// Handles admin-specific actions such as registering vendors.
    [ApiController]
    [Route("api/admin")]
    [Authorize(Policy = "AdminPolicy")]  // Only admins can access this controller
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

    
        /// Admin registers a new vendor account with a password.

        [HttpPost("register-vendor")]
        public async Task<IActionResult> RegisterVendor([FromBody] VendorRegistrationRequest vendorRequest)
        {
            try
            {
                // Pass individual fields to the service
                var registeredVendor = await _adminService.RegisterVendor(vendorRequest.Name, vendorRequest.Email, vendorRequest.Password);
                return Ok($"Vendor {registeredVendor.Email} registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    
        /// DTO for vendor registration, including password.

        public class VendorRegistrationRequest
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}