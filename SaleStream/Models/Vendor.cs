namespace SaleStream.Models
{
    public class Vendor
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();  // Auto-generated unique ID

        public string VendorName { get; set; }

        public string Email { get; set; }  // Unique email across both User and Vendor collections

        public string Password { get; set; }

        public string Role { get; set; } = "Vendor";  // Default role is Vendor

        public string Category { get; set; }

        public List<CommentEntry> Comments { get; set; } = new List<CommentEntry>();

        public int Status { get; set; } = 1;  // 1 = Active, 0 = Deactivated
    }

    public class CommentEntry
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();  // Unique ID for each comment

        public string UserId { get; set; }  // User ID of the commenter

        public string Comment { get; set; }  // The comment text

        public int Rank { get; set; }  // Rank value (1-5, for example)
    }
}
