using SaleStream.Models;
using SaleStream.Repositories;

namespace SaleStream.Services
{

    /// Handles business logic for Customer Service Representatives (CSR)
    public class CSRService
    {
        private readonly CSRRepository _csrRepository;

        public CSRService(CSRRepository csrRepository)
        {
            _csrRepository = csrRepository;
        }

    
        /// Retrieves all user accounts for the CSR
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _csrRepository.GetAllUsers();
        }

    
        /// Activates an inactive user account
        public async Task<User> ActivateUser(string id)
        {
            var user = await _csrRepository.GetUserById(id);
            if (user == null) return null;

            user.IsActive = true;
            await _csrRepository.UpdateUser(user);
            return user;
        }

    
        /// Reactivates a deactivated user account.
        public async Task<User> ReactivateUser(string id)
        {
            var user = await _csrRepository.GetUserById(id);
            if (user == null) return null;

            user.IsActive = true;
            await _csrRepository.UpdateUser(user);
            return user;
        }

    
        /// Retrieves all activated users for the CSR.
        public async Task<IEnumerable<User>> GetAllActivatedUsers()
        {
            return await _csrRepository.GetUsersByStatus(true);  // true means activated
        }

    
        /// Retrieves all deactivated users for the CSR.
        public async Task<IEnumerable<User>> GetAllDeactivatedUsers()
        {
            return await _csrRepository.GetUsersByStatus(false);  // false means deactivated
        }
    }
}
