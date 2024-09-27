using SaleStream.Models;
using SaleStream.Repositories;
using BCrypt.Net;

namespace SaleStream.Services
{

    /// Provides user management services such as updating, deleting, and deactivating users
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

    
        /// Retrieves a user by their ID
        public async Task<User> GetUserById(string id) => 
            await _userRepository.GetUserById(id) ?? null;

    
        /// Updates user details such as username, email, and password
        public async Task<User> UpdateUser(string id, User updatedUser)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return null;

            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;

            if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedUser.PasswordHash);
            }

            await _userRepository.UpdateUser(user);
            return user;
        }

    
        /// Deletes a user by their ID
        public async Task<bool> DeleteUser(string id) =>
            await _userRepository.DeleteUser(id);

    
        /// Deactivates a user by setting IsActive to false
        public async Task<User> DeactivateUser(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return null;

            user.IsActive = false;
            await _userRepository.UpdateUser(user);
            return user;
        }
    }
}
