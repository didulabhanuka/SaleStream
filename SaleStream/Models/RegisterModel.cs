/*namespace SaleStream.Models
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
*/

namespace SaleStream.Models
{
    public class RegisterModel
    {
        public string Email { get; set; }  // User's email address
        public string Password { get; set; }  // User's password
        public string Role { get; set; } // User role (e.g., Admin, User)
    }
}

