namespace SaleStream.Models
{

    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();  
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }  // Link to the Category
        public bool IsActive { get; set; } = true;  // Product is active by default
        public int Quantity { get; set; }  
    }
}
