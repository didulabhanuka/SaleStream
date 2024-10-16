using System;
using System.Collections.Generic;

namespace SaleStream.Models
{
    public class ShortNotification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();  // Auto-generated ID
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Automatically store the time

        public bool Removed { get; set; } = false;  // New attribute, default value is false
    }
}
