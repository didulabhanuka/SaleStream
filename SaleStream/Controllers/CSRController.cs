using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleStream.Models;
using SaleStream.Services;

namespace SaleStream.Controllers
{
    /// <summary>
    /// Handles operations for Customer Service Representatives (CSR).
    /// </summary>
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

        /// <summary>
        /// CSR can view all user accounts.
        /// </summary>
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _csrService.GetAllUsers();
            return Ok(users);
        }

        /// <summary>
        /// CSR can activate newly registered inactive users.
        /// </summary>
        [HttpPut("activate/{id}")]
        public async Task<IActionResult> ActivateUser(string id)
        {
            var user = await _csrService.ActivateUser(id);
            if (user == null) return NotFound("User not found or already active.");

            return Ok("User account activated successfully.");
        }

        /// <summary>
        /// CSR can reactivate deactivated users.
        /// </summary>
        [HttpPut("reactivate/{id}")]
        public async Task<IActionResult> ReactivateUser(string id)
        {
            var user = await _csrService.ReactivateUser(id);
            if (user == null) return NotFound("User not found or already active.");

            return Ok("User account reactivated successfully.");
        }
    }
}
