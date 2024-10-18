using System;
using System.Collections.Generic;

namespace EADWebApplication.Models
{
    public class Notification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();  // Auto-generated ID
        public string UserId { get; set; }  // User who made the request
        public string Email { get; set; }  // Email of the user
        public string OrderId { get; set; }  // Order Id related to the request
        public string Message { get; set; }  // Notification message
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
