using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleStream.Models;
using SaleStream.Services;

namespace SaleStream.Controllers
{
    /// <summary>
    /// Handles vendor-specific actions such as updating and deactivating their accounts.
    /// </summary>
    [ApiController]
    [Route("api/vendor")]
    [Authorize(Policy = "VendorPolicy")]  // Only vendors can access this controller
    public class VendorController : ControllerBase
    {
        private readonly VendorService _vendorService;

        public VendorController(VendorService vendorService)
        {
            _vendorService = vendorService;
        }

        /// <summary>
        /// Vendor updates their profile.
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateVendor([FromBody] Vendor updatedVendor)
        {
            try
            {
                var vendor = await _vendorService.UpdateVendor(updatedVendor);
                if (vendor == null)
                    return NotFound("Vendor not found.");
                return Ok("Vendor profile updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Vendor deactivates their account.
        /// </summary>
        [HttpPut("deactivate")]
        public async Task<IActionResult> DeactivateVendor(string vendorId)
        {
            var isDeactivated = await _vendorService.DeactivateVendor(vendorId);
            if (!isDeactivated)
                return NotFound("Vendor not found or already deactivated.");
            return Ok("Vendor account deactivated successfully.");
        }
    }
}
