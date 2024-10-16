namespace SaleStream.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "User";  // Default role
        public int Status { get; set; } = 1;  // 1 = Active, 0 = Deactivated
    }
}
