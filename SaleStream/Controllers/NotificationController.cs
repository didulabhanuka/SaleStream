using Microsoft.AspNetCore.Mvc;
using SaleStream.Services;
using SaleStream.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SaleStream.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // ------------------------- Cancel Notification Endpoints -------------------------

        [Authorize]
        [HttpGet("user-cancel-notifications")]
        public async Task<IActionResult> GetCancelNotifications()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID is missing from token.");
            }

            var notifications = await _notificationService.GetCancelNotificationsByUserIdAsync(userId);
            return Ok(notifications);
        }

        [Authorize]
        [HttpPost("create-cancel-request")]
        public async Task<IActionResult> CreateCancelRequestNotification([FromBody] CreateCancelRequestModel request)
        {
            await _notificationService.CreateCancelRequestNotificationAsync(request.OrderId, request.UserId, request.Email, "Cancel Request");
            return Ok("Cancellation request notification created.");
        }

        [Authorize(Roles = "Customer Service Representative, Admin")]
        [HttpGet("cancel-requests")]
        public async Task<IActionResult> GetAllCancelRequests()
        {
            var notifications = await _notificationService.GetAllCancelRequestsAsync();
            return Ok(notifications);
        }

        public class CreateCancelRequestModel
        {
            public string OrderId { get; set; }
            public string UserId { get; set; }
            public string Email { get; set; }
        }

        // ------------------------- Product Notification Endpoints -------------------------

        [Authorize(Roles = "Customer Service Representative, Admin")]
        [HttpGet("product-notifications")]
        public async Task<IActionResult> GetAllProductNotifications()
        {
            var notifications = await _notificationService.GetAllProductNotificationsAsync();
            return Ok(notifications);
        }

        [Authorize(Roles = "Customer Service Representative, Admin")]
        [HttpGet("product-notification/{id}")]
        public async Task<IActionResult> GetProductNotificationById(string id)
        {
            var notification = await _notificationService.GetProductNotificationByIdAsync(id);
            if (notification == null)
            {
                return NotFound("Notification not found.");
            }
            return Ok(notification);
        }

        [Authorize(Roles = "Vendor")]
        [HttpGet("vendor-product-notifications")]
        public async Task<IActionResult> GetProductNotificationsByVendorEmail()
        {
            var vendorEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var notifications = await _notificationService.GetProductNotificationsByVendorEmailAsync(vendorEmail);
            return Ok(notifications);
        }

        [Authorize(Roles = "Vendor")]
        [HttpPut("remove-product-notification/{id}")]
        public async Task<IActionResult> MarkProductNotificationAsRemoved(string id, [FromBody] RemovedModel model)
        {
            await _notificationService.MarkProductNotificationAsRemovedAsync(id, model.Removed);
            return Ok("Notification updated successfully.");
        }

        public class RemovedModel
        {
            public bool Removed { get; set; }
        }

        // ------------------------- Short Notification Endpoints -------------------------

        [Authorize(Roles = "Customer Service Representative, Admin")]
        [HttpGet("short-notifications")]
        public async Task<IActionResult> GetAllShortNotifications()
        {
            var notifications = await _notificationService.GetAllShortNotificationsAsync();
            return Ok(notifications);
        }

        [Authorize]
        [HttpGet("user-short-notifications")]
        public async Task<IActionResult> GetShortNotificationsByUserId()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var notifications = await _notificationService.GetNonRemovedShortNotificationsByUserIdAsync(userId);
            return Ok(notifications);
        }

        [Authorize]
        [HttpPut("remove-short-notification/{id}")]
        public async Task<IActionResult> RemoveShortNotification(string id)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var notification = await _notificationService.GetShortNotificationByIdAsync(id);

            if (notification == null)
                return NotFound("Notification not found.");

            if (notification.UserId != userId)
                return Unauthorized("You can only modify your own notifications.");

            notification.Removed = true;  // Mark the notification as removed
            await _notificationService.UpdateShortNotificationAsync(notification);

            return Ok("Notification marked as removed.");
        }
    }
}
