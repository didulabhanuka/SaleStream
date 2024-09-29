using MongoDB.Driver;
using SaleStream.Models;

namespace SaleStream.Repositories
{

    /// Data access for Customer Service Representatives (CSR) operations.
    public class CSRRepository
    {
        private readonly IMongoCollection<User> _users;

        public CSRRepository(IMongoClient client)
        {
            var database = client.GetDatabase("salestream");
            _users = database.GetCollection<User>("Users");
        }

    
        /// Retrieves all users for the CSR to view.
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

    
        /// Retrieves a user by their ID.
        public async Task<User> GetUserById(string id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

    
        /// Updates a user's status, such as activation or reactivation.
        public async Task UpdateUser(User user)
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

    
        /// Retrieves users based on their activation status (true = activated, false = deactivated).
        public async Task<IEnumerable<User>> GetUsersByStatus(bool isActive)
        {
            return await _users.Find(u => u.IsActive == isActive).ToListAsync();
        }
    }
}
