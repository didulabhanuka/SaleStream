using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleStream.Models;
using SaleStream.Services;

namespace SaleStream.Controllers
{

    /// Handles operations for Customer Service Representatives (CSR).
    [ApiController]
    [Route("api/csr")]
    [Authorize(Policy = "CSRPolicy")]  // Only CSR can access these routes
    public class CSRController : ControllerBase
    {
        private readonly CSRService _csrService;

        public CSRController(CSRService csrService)
        {
            _csrService = csrService;
        }

    
        /// CSR can view all user accounts.

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _csrService.GetAllUsers();
            return Ok(users);
        }

    
        /// CSR can activate newly registered inactive users.

        [HttpPut("activate/{id}")]
        public async Task<IActionResult> ActivateUser(string id)
        {
            var user = await _csrService.ActivateUser(id);
            if (user == null) return NotFound("User not found or already active.");

            return Ok("User account activated successfully.");
        }

    
        /// CSR can reactivate deactivated users.

        [HttpPut("reactivate/{id}")]
        public async Task<IActionResult> ReactivateUser(string id)
        {
            var user = await _csrService.ReactivateUser(id);
            if (user == null) return NotFound("User not found or already active.");

            return Ok("User account reactivated successfully.");
        }

    
        /// CSR can view all activated user accounts.

        [HttpGet("activated-users")]
        public async Task<IActionResult> GetAllActivatedUsers()
        {
            var users = await _csrService.GetAllActivatedUsers();
            if (users == null || !users.Any())
                return NotFound("No activated users found.");
            
            return Ok(users);
        }

    
        /// CSR can view all deactivated user accounts.

        [HttpGet("deactivated-users")]
        public async Task<IActionResult> GetAllDeactivatedUsers()
        {
            var users = await _csrService.GetAllDeactivatedUsers();
            if (users == null || !users.Any())
                return NotFound("No deactivated users found.");

            return Ok(users);
        }
    }
}
