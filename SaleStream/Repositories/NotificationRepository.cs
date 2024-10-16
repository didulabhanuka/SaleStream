using MongoDB.Driver;
using SaleStream.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaleStream.Repositories
{
    public class NotificationRepository
    {
        private readonly IMongoCollection<CancelNotification> _cancelNotifications;
        private readonly IMongoCollection<ProductNotification> _productNotifications;
        private readonly IMongoCollection<ShortNotification> _shortNotifications;

        public NotificationRepository(IMongoClient client)
        {
            var database = client.GetDatabase("salestream");
            _cancelNotifications = database.GetCollection<CancelNotification>("CancelNotifications");
            _productNotifications = database.GetCollection<ProductNotification>("ProductNotifications");
            _shortNotifications = database.GetCollection<ShortNotification>("ShortNotifications");
        }

        // ------------------------- Cancel Notifications -------------------------

        public async Task InsertCancelNotificationAsync(CancelNotification notification)
        {
            await _cancelNotifications.InsertOneAsync(notification);
        }

        public async Task<List<CancelNotification>> GetCancelNotificationsByUserIdAsync(string userId)
        {
            return await _cancelNotifications.Find(n => n.UserId == userId).ToListAsync();
        }

        public async Task<List<CancelNotification>> GetAllCancelRequestsAsync()
        {
            return await _cancelNotifications.Find(n => n.Message.Contains("Cancel Request")).ToListAsync();
        }

        // ------------------------- Product Notifications -------------------------

        public async Task InsertProductNotificationAsync(ProductNotification notification)
        {
            await _productNotifications.InsertOneAsync(notification);
        }

        public async Task<List<ProductNotification>> GetAllProductNotificationsAsync()
        {
            return await _productNotifications.Find(_ => true).ToListAsync();
        }

        public async Task<ProductNotification> GetProductNotificationByIdAsync(string notificationId)
        {
            return await _productNotifications.Find(n => n.Id == notificationId).FirstOrDefaultAsync();
        }

        public async Task<List<ProductNotification>> GetProductNotificationsByVendorEmailAsync(string vendorEmail)
        {
            return await _productNotifications.Find(n => n.VendorEmail == vendorEmail && !n.Removed).ToListAsync();
        }

        public async Task UpdateProductNotificationAsync(ProductNotification notification)
        {
            await _productNotifications.ReplaceOneAsync(n => n.Id == notification.Id, notification);
        }

        // ------------------------- Short Notifications -------------------------

        public async Task InsertShortNotificationAsync(ShortNotification notification)
        {
            await _shortNotifications.InsertOneAsync(notification);
        }

        public async Task<List<ShortNotification>> GetAllShortNotificationsAsync()
        {
            return await _shortNotifications.Find(_ => true).ToListAsync();
        }

        public async Task<List<ShortNotification>> GetNonRemovedShortNotificationsByUserIdAsync(string userId)
        {
            return await _shortNotifications.Find(n => n.UserId == userId && !n.Removed).ToListAsync();
        }

        public async Task<ShortNotification> GetShortNotificationByIdAsync(string notificationId)
        {
            return await _shortNotifications.Find(n => n.Id == notificationId).FirstOrDefaultAsync();
        }

        public async Task UpdateShortNotificationAsync(ShortNotification notification)
        {
            await _shortNotifications.ReplaceOneAsync(n => n.Id == notification.Id, notification);
        }
    }
}
