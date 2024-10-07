using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleStream.Models;
using SaleStream.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SaleStream.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthController(UserService userService, JwtService jwtService, IConfiguration configuration)
        {
            _userService = userService;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!await _userService.IsEmailUniqueAsync(model.Email))
            {
                return BadRequest("Email already exists.");
            }

            var newUser = new User
            {
                Email = model.Email,
                Password = UserService.EncryptPassword(model.Password),
                Role = "User",  // Default role is User
                Status = 1  // Active by default
            };

            await _userService.CreateUserAsync(newUser);
            return Ok("User registered successfully.");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.GetUserByEmailAsync(model.Email);
            if (user == null || !UserService.VerifyPassword(model.Password, user.Password) || user.Status != 1)
            {
                return Unauthorized("Invalid credentials or inactive account.");
            }

            var token = _jwtService.GenerateToken(user);
            return Ok(new { Token = token, Role = user.Role });
        }

        [Authorize]
        [HttpPost("deactivate")]
        public async Task<IActionResult> DeactivateAccount()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Status = 0;
            await _userService.UpdateUserAsync(user);
            return Ok("Account deactivated.");
        }

        [Authorize(Roles = "Customer Service Representative")]
        [HttpPost("activate")]
        public async Task<IActionResult> ActivateAccount([FromBody] ActivateAccountRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("The email field is required.");
            }

            var user = await _userService.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Status = 1;
            await _userService.UpdateUserAsync(user);
            return Ok("Account activated.");
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null || !UserService.VerifyPassword(model.OldPassword, user.Password))
            {
                return BadRequest("Invalid credentials.");
            }

            user.Password = UserService.EncryptPassword(model.NewPassword);
            await _userService.UpdateUserAsync(user);
            return Ok("Password updated.");
        }

        [Authorize]
        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst("UserId")?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.Role == "Vendor")
            {
                return Forbid("Vendors cannot delete their own accounts.");
            }

            if (role == "Admin" || role == "Customer Service Representative")
            {
                await _userService.DeleteUserAsync(userId);
                return Ok("User account deleted by Admin/CSR.");
            }

            if (userId == user.Id)
            {
                await _userService.DeleteUserAsync(userId);
                return Ok("Your account has been deleted.");
            }

            return Forbid("Unauthorized to delete this account.");
        }

        [Authorize(Roles = "Admin, Customer Service Representative, Vendor")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin, Customer Service Representative, Vendor")]
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        public class ChangePasswordModel
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }

        public class ActivateAccountRequest
        {
            public string Email { get; set; }
        }
    }
}
