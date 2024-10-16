using System;
using System.Collections.Generic;

namespace SaleStream.Models
{
    public class ProductNotification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();  // Auto-generated ID

        public string ProductName { get; set; }
        public int AvailableQuantity { get; set; }
        public string VendorEmail { get; set; }
        public string VendorId { get; set; }
        public bool Removed { get; set; } = false;
        public string Message { get; set; }
        public DateTime NotificationDate { get; set; } = DateTime.Now;
    }
}

