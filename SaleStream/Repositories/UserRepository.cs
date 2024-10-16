using MongoDB.Driver;
using SaleStream.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaleStream.Repositories
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoClient client)
        {
            var database = client.GetDatabase("salestream");
            _users = database.GetCollection<User>("Users");
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await _users.Find(u => u.Email == email).AnyAsync() == false;
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _users.DeleteOneAsync(u => u.Id == userId);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _users.Find(user => true).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
        }
    }
}
