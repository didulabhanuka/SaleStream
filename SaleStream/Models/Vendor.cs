namespace SaleStream.Models
{
    public class Vendor
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();  // Unique Vendor ID
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } 
        public string Role { get; set; } = "Vendor";  // Default role for vendors
        public bool IsActive { get; set; } = true;  // Active by default
    }
}
