using SaleStream.Models;
using SaleStream.Repositories;

namespace SaleStream.Services
{
    /// <summary>
    /// Handles business logic for Customer Service Representatives (CSR).
    /// </summary>
    public class CSRService
    {
        private readonly CSRRepository _csrRepository;

        public CSRService(CSRRepository csrRepository)
        {
            _csrRepository = csrRepository;
        }

        /// <summary>
        /// Retrieves all user accounts for the CSR.
        /// </summary>
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _csrRepository.GetAllUsers();
        }

        /// <summary>
        /// Activates an inactive user account.
        /// </summary>
        public async Task<User> ActivateUser(string id)
        {
            var user = await _csrRepository.GetUserById(id);
            if (user == null) return null;

            user.IsActive = true;
            await _csrRepository.UpdateUser(user);
            return user;
        }

        /// <summary>
        /// Reactivates a deactivated user account.
        /// </summary>
        public async Task<User> ReactivateUser(string id)
        {
            var user = await _csrRepository.GetUserById(id);
            if (user == null) return null;

            user.IsActive = true;
            await _csrRepository.UpdateUser(user);
            return user;
        }
    }
}
