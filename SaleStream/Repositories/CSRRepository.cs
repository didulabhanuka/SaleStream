using MongoDB.Driver;
using SaleStream.Models;

namespace SaleStream.Repositories
{
    /// <summary>
    /// Data access for Customer Service Representatives (CSR) operations.
    /// </summary>
    public class CSRRepository
    {
        private readonly IMongoCollection<User> _users;

        public CSRRepository(IMongoClient client)
        {
            var database = client.GetDatabase("salestream");
            _users = database.GetCollection<User>("Users");
        }

        /// <summary>
        /// Retrieves all users for the CSR to view.
        /// </summary>
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        public async Task<User> GetUserById(string id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates a user's status, such as activation or reactivation.
        /// </summary>
        public async Task UpdateUser(User user)
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }
    }
}
