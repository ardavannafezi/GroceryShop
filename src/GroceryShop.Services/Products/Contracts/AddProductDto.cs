namespace GroceryShop.Services.Categories.Contracts
{
    public class AddProductDto
    {
        public int ProductCode { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public int MaxInStock { get; set; }
        public int MinInStock { get; set; }
    }
}
