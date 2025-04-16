namespace TestEFCore.Models;

public class Product
{
        // Primary key.
        public int Id { get; set; }
        // Name of the product.
        public string Name { get; set; } = string.Empty;
        // Price of the product.
        public decimal Price { get; set; }
}