using SaleStream.Models;
using SaleStream.Repositories;
using BCrypt.Net;  // For password hashing

namespace SaleStream.Services
{

    /// Admin service for registering vendor accounts.
    public class AdminService
    {
        private readonly AdminRepository _adminRepository;

        public AdminService(AdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

    
        /// Registers a new vendor account with a password.
        public async Task<Vendor> RegisterVendor(string name, string email, string password)
        {
            // Ensure the vendor email does not already exist
            var existingVendor = await _adminRepository.GetVendorByEmail(email);
            if (existingVendor != null)
                throw new Exception("Vendor with this email already exists.");

            // Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Register the new vendor with default "Vendor" role
            var vendor = new Vendor
            {
                Name = name,
                Email = email,
                PasswordHash = hashedPassword,  // Store the hashed password
                Role = "Vendor"
            };

            await _adminRepository.CreateVendor(vendor);
            return vendor;
        }
    }
}
