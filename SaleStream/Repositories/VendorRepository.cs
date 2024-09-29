using MongoDB.Driver;
using SaleStream.Models;

namespace SaleStream.Repositories
{
    public class VendorRepository
    {
        private readonly IMongoCollection<Vendor> _vendors;

        public VendorRepository(IMongoClient client)
        {
            var database = client.GetDatabase("salestream");
            _vendors = database.GetCollection<Vendor>("Vendors");
        }

        /// <summary>
        /// Retrieves a vendor by their email.
        /// </summary>
        public async Task<Vendor> GetVendorByEmail(string email)
        {
            return await _vendors.Find(v => v.Email == email).FirstOrDefaultAsync();
        }
        
        /// <summary>
        /// Updates an existing vendor account.
        /// </summary>
        public async Task UpdateVendor(Vendor vendor)
        {
            await _vendors.ReplaceOneAsync(v => v.Id == vendor.Id, vendor);
        }

        /// <summary>
        /// Deactivates a vendor account by setting IsActive to false.
        /// </summary>
        public async Task<bool> DeactivateVendor(string vendorId)
        {
            var update = Builders<Vendor>.Update.Set(v => v.IsActive, false);
            var result = await _vendors.UpdateOneAsync(v => v.Id == vendorId, update);
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Retrieves a vendor by their ID.
        /// </summary>
        public async Task<Vendor> GetVendorById(string vendorId)
        {
            return await _vendors.Find(v => v.Id == vendorId).FirstOrDefaultAsync();
        }
    }
}
