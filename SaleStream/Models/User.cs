namespace SaleStream.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "User";  // Role is always "User" by default
        public bool IsActive { get; set; } = true;  // New field to manage deactivation
    }
}
