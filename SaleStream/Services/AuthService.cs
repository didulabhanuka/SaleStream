using SaleStream.Models;
using SaleStream.Repositories;
using BCrypt.Net;

namespace SaleStream.Services
{

    /// Manages authentication and registration logic for both users and vendors.
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly VendorRepository _vendorRepository;

        public AuthService(UserRepository userRepository, VendorRepository vendorRepository)
        {
            _userRepository = userRepository;
            _vendorRepository = vendorRepository;
        }

    
        /// Registers a new user with hashed password and inactive status by default.
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
                Role = "User",  // Default role for users
                IsActive = false  // Newly registered users are inactive by default
            };

            await _userRepository.CreateUser(user);
        }

    
        /// Authenticates a user or vendor based on email and password.
        public async Task<User> AuthenticateUser(string email, string password)
        {
            // First, check if the email matches a user
            var user = await _userRepository.GetUserByEmail(email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;  // Return the user if found
            }

            // If not found, check if the email matches a vendor
            var vendor = await _vendorRepository.GetVendorByEmail(email);
            if (vendor != null && BCrypt.Net.BCrypt.Verify(password, vendor.PasswordHash))
            {
                // Map the Vendor to a User object
                return new User
                {
                    Id = vendor.Id,
                    Email = vendor.Email,
                    PasswordHash = vendor.PasswordHash,  // Keep the hashed password
                    Role = "Vendor",  // Assign the Vendor role
                    IsActive = vendor.IsActive
                };
            }

            return null;  // Return null if no user or vendor is found
        }
    }
}
