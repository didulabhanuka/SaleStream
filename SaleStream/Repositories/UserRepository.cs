using MongoDB.Driver;
using SaleStream.Models;

namespace SaleStream.Repositories
{

    /// Handles database interactions for User entities
    public class UserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoClient client)
        {
            var database = client.GetDatabase("salestream");
            _users = database.GetCollection<User>("Users");
        }


        /// Creates a new user in the database
        public async Task CreateUser(User user) => 
            await _users.InsertOneAsync(user);


        /// Retrieves a user by their email
        public async Task<User> GetUserByEmail(string email) => 
            await _users.Find(u => u.Email == email).FirstOrDefaultAsync();


        /// Retrieves a user by their ID
        public async Task<User> GetUserById(string id) => 
            await _users.Find(u => u.Id == id).FirstOrDefaultAsync();


        /// Updates user details in the database
        public async Task UpdateUser(User user) => 
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);


        /// Deletes a user by their ID
        public async Task<bool> DeleteUser(string id)
        {
            var result = await _users.DeleteOneAsync(u => u.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
