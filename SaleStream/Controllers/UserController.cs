using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleStream.Models;
using SaleStream.Services;

namespace SaleStream.Controllers
{

    /// Manages user-related operations such as getting, updating, and deleting users
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }


        /// Retrieves a user by their ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null) return NotFound("User not found");
            return Ok(user);
        }


        /// Updates a user's details
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(string id, User updatedUser)
        {
            var user = await _userService.UpdateUser(id, updatedUser);
            if (user == null) return NotFound("User not found");
            return Ok(user);
        }


        /// Deletes a user by their ID
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var isDeleted = await _userService.DeleteUser(id);
            if (!isDeleted) return NotFound("User not found");
            return Ok("User deleted successfully");
        }


        /// Deactivates a user by setting their IsActive status to false
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivateUser(string id)
        {
            var deactivatedUser = await _userService.DeactivateUser(id);
            if (deactivatedUser == null) return NotFound("User not found");
            return Ok("User deactivated successfully");
        }
    }
}
