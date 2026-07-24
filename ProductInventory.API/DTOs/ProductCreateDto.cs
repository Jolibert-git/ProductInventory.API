namespace ProductInventory.API.DTOs
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int QuantityStock { get; set; }
        public string Category { get; set; } = null!;
    }
}
