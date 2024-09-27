using SaleStream.Models;
using SaleStream.Repositories;
using BCrypt.Net;

namespace SaleStream.Services
{

    /// Manages authentication and registration logic
    public class AuthService
    {
        private readonly UserRepository _userRepository;

        public AuthService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

    
        /// Registers a new user with hashed password
        public async Task RegisterUser(string username, string email, string password)
        {
            var existingUser = await _userRepository.GetUserByEmail(email);
            if (existingUser != null)
                throw new Exception("Email already exists!");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                Role = "User"  // Default role
            };

            await _userRepository.CreateUser(user);
        }

    
        /// Authenticates a user based on email and password
        public async Task<User> AuthenticateUser(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return user;
        }
    }
}
