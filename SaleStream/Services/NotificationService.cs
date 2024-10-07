using SaleStream.Models;
using SaleStream.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaleStream.Services
{
    public class NotificationService
    {
        private readonly NotificationRepository _notificationRepository;

        public NotificationService(NotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        // ------------------------- Cancel Notification Methods -------------------------

        public async Task NotifyCustomerAsync(string userId, string email, string orderId, string message)
        {
            var notification = new CancelNotification
            {
                UserId = userId,
                Email = email,
                OrderId = orderId,
                Message = message
            };

            await _notificationRepository.InsertCancelNotificationAsync(notification);
        }

        public async Task<List<CancelNotification>> GetCancelNotificationsByUserIdAsync(string userId)
        {
            return await _notificationRepository.GetCancelNotificationsByUserIdAsync(userId);
        }

        public async Task CreateCancelRequestNotificationAsync(string orderId, string userId, string email, string message)
        {
            var notification = new CancelNotification
            {
                UserId = userId,
                Email = email,
                OrderId = orderId,
                Message = message
            };

            await _notificationRepository.InsertCancelNotificationAsync(notification);
        }

        public async Task<List<CancelNotification>> GetAllCancelRequestsAsync()
        {
            return await _notificationRepository.GetAllCancelRequestsAsync();
        }

        // ------------------------- Product Notification Methods -------------------------

        public async Task CreateProductNotificationAsync(ProductNotification notification)
        {
            await _notificationRepository.InsertProductNotificationAsync(notification);
        }

        public async Task<List<ProductNotification>> GetAllProductNotificationsAsync()
        {
            return await _notificationRepository.GetAllProductNotificationsAsync();
        }

        public async Task<ProductNotification> GetProductNotificationByIdAsync(string notificationId)
        {
            return await _notificationRepository.GetProductNotificationByIdAsync(notificationId);
        }

        public async Task<List<ProductNotification>> GetProductNotificationsByVendorEmailAsync(string vendorEmail)
        {
            return await _notificationRepository.GetProductNotificationsByVendorEmailAsync(vendorEmail);
        }

        public async Task MarkProductNotificationAsRemovedAsync(string notificationId, bool removed)
        {
            var notification = await _notificationRepository.GetProductNotificationByIdAsync(notificationId);
            if (notification != null)
            {
                notification.Removed = removed;
                await _notificationRepository.UpdateProductNotificationAsync(notification);
            }
        }

        // ------------------------- Short Notification Methods -------------------------

        public async Task CreateShortNotificationAsync(ShortNotification notification)
        {
            await _notificationRepository.InsertShortNotificationAsync(notification);
        }

        public async Task<List<ShortNotification>> GetAllShortNotificationsAsync()
        {
            return await _notificationRepository.GetAllShortNotificationsAsync();
        }

        public async Task<List<ShortNotification>> GetNonRemovedShortNotificationsByUserIdAsync(string userId)
        {
            return await _notificationRepository.GetNonRemovedShortNotificationsByUserIdAsync(userId);
        }

        public async Task<ShortNotification> GetShortNotificationByIdAsync(string notificationId)
        {
            return await _notificationRepository.GetShortNotificationByIdAsync(notificationId);
        }

        public async Task UpdateShortNotificationAsync(ShortNotification notification)
        {
            await _notificationRepository.UpdateShortNotificationAsync(notification);
        }

        public async Task MarkShortNotificationAsRemovedAsync(string notificationId)
        {
            var notification = await _notificationRepository.GetShortNotificationByIdAsync(notificationId);
            if (notification != null)
            {
                notification.Removed = true;
                await _notificationRepository.UpdateShortNotificationAsync(notification);
            }
        }
    }
}
