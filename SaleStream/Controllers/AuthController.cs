using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleStream.Models;
using SaleStream.Services;
using Microsoft.Extensions.Configuration;

namespace SaleStream.Controllers
{

    /// Handles authentication and authorization
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthController(AuthService authService, JwtService jwtService, IConfiguration configuration)
        {
            _authService = authService;
            _jwtService = jwtService;
            _configuration = configuration;
        }


        /// Registers a new user
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                await _authService.RegisterUser(user.Username, user.Email, user.PasswordHash);
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// Authenticates and logs in the user
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Check predefined users from appsettings.json
            var predefinedUsersSection = _configuration.GetSection("PredefinedUsers");
            User predefinedUser = null;

            if (email == predefinedUsersSection.GetSection("Administrator:Email").Value &&
                password == predefinedUsersSection.GetSection("Administrator:Password").Value)
            {
                predefinedUser = new User
                {
                    Id = predefinedUsersSection.GetSection("Administrator:Id").Value,
                    Email = email,
                    Role = predefinedUsersSection.GetSection("Administrator:Role").Value
                };
            }
            else if (email == predefinedUsersSection.GetSection("CSR:Email").Value &&
                     password == predefinedUsersSection.GetSection("CSR:Password").Value)
            {
                predefinedUser = new User
                {
                    Id = predefinedUsersSection.GetSection("CSR:Id").Value,
                    Email = email,
                    Role = predefinedUsersSection.GetSection("CSR:Role").Value
                };
            }

            if (predefinedUser != null)
            {
                var token = _jwtService.GenerateToken(predefinedUser);
                return Ok(new
                {
                    token,
                    userId = predefinedUser.Id,
                    email = predefinedUser.Email,
                    role = predefinedUser.Role
                });
            }

            // Regular user authentication
            var user = await _authService.AuthenticateUser(email, password);
            if (user == null) return Unauthorized();

            var tokenRegular = _jwtService.GenerateToken(user);
            return Ok(new
            {
                token = tokenRegular,
                userId = user.Id,
                email = user.Email,
                role = user.Role
            });
        }
    }
}
