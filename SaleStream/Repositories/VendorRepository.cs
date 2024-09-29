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


        /// Retrieves a vendor by their email.
        public async Task<Vendor> GetVendorByEmail(string email)
        {
            return await _vendors.Find(v => v.Email == email).FirstOrDefaultAsync();
        }
        

        /// Updates an existing vendor account.
        public async Task UpdateVendor(Vendor vendor)
        {
            await _vendors.ReplaceOneAsync(v => v.Id == vendor.Id, vendor);
        }


        /// Deactivates a vendor account by setting IsActive to false.
        public async Task<bool> DeactivateVendor(string vendorId)
        {
            var update = Builders<Vendor>.Update.Set(v => v.IsActive, false);
            var result = await _vendors.UpdateOneAsync(v => v.Id == vendorId, update);
            return result.ModifiedCount > 0;
        }


        /// Retrieves a vendor by their ID.
        public async Task<Vendor> GetVendorById(string vendorId)
        {
            return await _vendors.Find(v => v.Id == vendorId).FirstOrDefaultAsync();
        }
    }
}
