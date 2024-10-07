using SaleStream.Models;
using SaleStream.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SaleStream.Services
{
    public class VendorService
    {
        private readonly VendorRepository _vendorRepository;
        private readonly UserService _userService; // Used to sync between Vendor and User collections

        public VendorService(VendorRepository vendorRepository, UserService userService)
        {
            _vendorRepository = vendorRepository;
            _userService = userService;
        }

        public async Task<Vendor> GetVendorByEmailAsync(string email)
        {
            return await _vendorRepository.GetVendorByEmailAsync(email);
        }

        public async Task CreateVendorAsync(Vendor vendor)
        {
            await _vendorRepository.CreateVendorAsync(vendor);

            // Create an entry in the User collection for login purposes
            var newUser = new User
            {
                Email = vendor.Email,
                Password = vendor.Password,  // Should be encrypted
                Role = "Vendor",
                Status = 1
            };
            await _userService.CreateUserAsync(newUser);
        }

        public async Task UpdateVendorAsync(Vendor vendor)
        {
            await _vendorRepository.UpdateVendorAsync(vendor);

            // Sync the changes to the User collection (for email/password updates)
            var user = await _userService.GetUserByEmailAsync(vendor.Email);
            if (user != null)
            {
                user.Email = vendor.Email;
                user.Password = vendor.Password;
                await _userService.UpdateUserAsync(user);
            }
        }

        public async Task DeleteVendorAsync(string email)
        {
            await _vendorRepository.DeleteVendorAsync(email);

            // Delete from User collection as well
            var user = await _userService.GetUserByEmailAsync(email);
            if (user != null)
            {
                await _userService.DeleteUserAsync(user.Id);
            }
        }

        public async Task<List<Vendor>> GetVendorsAsync()
        {
            return await _vendorRepository.GetVendorsAsync();
        }

        public async Task AddCommentAsync(string vendorId, string comment, int rank, string userId)
        {
            await _vendorRepository.AddCommentAsync(vendorId, comment, rank, userId);
        }

        public async Task UpdateCommentAsync(string vendorId, string commentId, string newComment, int newRank, string userId)
        {
            await _vendorRepository.UpdateCommentAsync(vendorId, commentId, newComment, newRank, userId);
        }

        public async Task DeleteCommentAsync(string vendorId, string commentId, string userId)
        {
            await _vendorRepository.DeleteCommentAsync(vendorId, commentId, userId);
        }

        public async Task<Vendor> GetVendorByIdAsync(string vendorId)
        {
            return await _vendorRepository.GetVendorByIdAsync(vendorId);
        }
    }
}
