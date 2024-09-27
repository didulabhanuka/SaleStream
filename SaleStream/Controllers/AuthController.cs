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

    
        /// Registers a new user with inactive status by default.

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                await _authService.RegisterUser(user.Username, user.Email, user.PasswordHash);
                return Ok("User registered successfully. Your account is pending activation.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    
        /// Logs in a user using email and password via JSON.

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginData loginData)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(loginData.Email) || string.IsNullOrEmpty(loginData.Password))
            {
                return BadRequest("Email and password are required.");
            }

            // Check predefined users from appsettings.json
            var predefinedUsersSection = _configuration.GetSection("PredefinedUsers");
            User predefinedUser = null;

            // Check if the login matches predefined users (Administrator/CSR)
            if (loginData.Email == predefinedUsersSection.GetSection("Administrator:Email").Value &&
                loginData.Password == predefinedUsersSection.GetSection("Administrator:Password").Value)
            {
                predefinedUser = new User
                {
                    Id = predefinedUsersSection.GetSection("Administrator:Id").Value,
                    Email = loginData.Email,
                    Role = predefinedUsersSection.GetSection("Administrator:Role").Value
                };
            }
            else if (loginData.Email == predefinedUsersSection.GetSection("CSR:Email").Value &&
                     loginData.Password == predefinedUsersSection.GetSection("CSR:Password").Value)
            {
                predefinedUser = new User
                {
                    Id = predefinedUsersSection.GetSection("CSR:Id").Value,
                    Email = loginData.Email,
                    Role = predefinedUsersSection.GetSection("CSR:Role").Value
                };
            }

            // If a predefined user is found, generate and return a token
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

            // Authenticate regular users
            var user = await _authService.AuthenticateUser(loginData.Email, loginData.Password);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            if (!user.IsActive)
                return BadRequest("Your account is still pending activation.");

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


    /// LoginData class to capture email and password from JSON body.
    public class LoginData
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
