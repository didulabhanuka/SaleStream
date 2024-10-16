using MongoDB.Driver;
using SaleStream.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson;

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

        public async Task<Vendor> GetVendorByEmailAsync(string email)
        {
            return await _vendors.Find(v => v.Email == email).FirstOrDefaultAsync();
        }

        public async Task CreateVendorAsync(Vendor vendor)
        {
            await _vendors.InsertOneAsync(vendor);
        }

        public async Task UpdateVendorAsync(Vendor vendor)
        {
            await _vendors.ReplaceOneAsync(v => v.Id == vendor.Id, vendor);
        }

        public async Task DeleteVendorAsync(string email)
        {
            await _vendors.DeleteOneAsync(v => v.Email == email);
        }

        public async Task<List<Vendor>> GetVendorsAsync()
        {
            return await _vendors.Find(_ => true).ToListAsync();
        }

        public async Task AddCommentAsync(string vendorId, string comment, int rank, string userId)
        {
            var vendor = await _vendors.Find(v => v.Id == vendorId).FirstOrDefaultAsync();
            if (vendor != null)
            {
                var newComment = new CommentEntry
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    UserId = userId,
                    Comment = comment,
                    Rank = rank
                };

                vendor.Comments.Add(newComment);
                await _vendors.ReplaceOneAsync(v => v.Id == vendor.Id, vendor);
            }
        }

        public async Task UpdateCommentAsync(string vendorId, string commentId, string newComment, int newRank, string userId)
        {
            var vendor = await _vendors.Find(v => v.Id == vendorId).FirstOrDefaultAsync();
            if (vendor != null)
            {
                var comment = vendor.Comments.FirstOrDefault(c => c.Id == commentId && c.UserId == userId);
                if (comment != null)
                {
                    comment.Comment = newComment;
                    comment.Rank = newRank;
                    await _vendors.ReplaceOneAsync(v => v.Id == vendor.Id, vendor);
                }
            }
        }

        public async Task DeleteCommentAsync(string vendorId, string commentId, string userId)
        {
            var vendor = await _vendors.Find(v => v.Id == vendorId).FirstOrDefaultAsync();
            if (vendor != null)
            {
                vendor.Comments.RemoveAll(c => c.Id == commentId && c.UserId == userId);
                await _vendors.ReplaceOneAsync(v => v.Id == vendor.Id, vendor);
            }
        }

        public async Task<Vendor> GetVendorByIdAsync(string vendorId)
        {
            return await _vendors.Find(v => v.Id == vendorId).FirstOrDefaultAsync();
        }
    }
}
