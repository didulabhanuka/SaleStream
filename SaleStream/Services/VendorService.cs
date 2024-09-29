using SaleStream.Models;
using SaleStream.Repositories;

namespace SaleStream.Services
{
    /// <summary>
    /// Vendor service for updating and deactivating vendor accounts.
    /// </summary>
    public class VendorService
    {
        private readonly VendorRepository _vendorRepository;

        public VendorService(VendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        /// <summary>
        /// Updates the vendor profile.
        /// </summary>
        public async Task<Vendor> UpdateVendor(Vendor updatedVendor)
        {
            var vendor = await _vendorRepository.GetVendorById(updatedVendor.Id);
            if (vendor == null) return null;

            vendor.Name = updatedVendor.Name;
            vendor.Email = updatedVendor.Email;

            await _vendorRepository.UpdateVendor(vendor);
            return vendor;
        }

        /// <summary>
        /// Deactivates the vendor account.
        /// </summary>
        public async Task<bool> DeactivateVendor(string vendorId)
        {
            return await _vendorRepository.DeactivateVendor(vendorId);
        }
    }
}
