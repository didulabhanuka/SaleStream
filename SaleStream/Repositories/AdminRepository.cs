using MongoDB.Driver;
using SaleStream.Models;

namespace SaleStream.Repositories
{
    public class AdminRepository
    {
        private readonly IMongoCollection<Vendor> _vendors;

        public AdminRepository(IMongoClient client)
        {
            var database = client.GetDatabase("salestream");
            _vendors = database.GetCollection<Vendor>("Vendors");
        }

        /// <summary>
        /// Creates a new vendor account.
        /// </summary>
        public async Task CreateVendor(Vendor vendor)
        {
            await _vendors.InsertOneAsync(vendor);
        }

        /// <summary>
        /// Retrieves a vendor by email.
        /// </summary>
        public async Task<Vendor> GetVendorByEmail(string email)
        {
            return await _vendors.Find(v => v.Email == email).FirstOrDefaultAsync();
        }
    }
}
