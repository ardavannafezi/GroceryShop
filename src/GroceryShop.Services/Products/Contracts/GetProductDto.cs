namespace GroceryShop.Services.Products.Contracts
{
    public class GetProductDto
    {
        public int ProductCode { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public double BuyPrice { get; set; }
        public double SellPrice { get; set; }
        public int Quantity { get; set; }
        public int? MaxInStock { get; set; }
        public int? MinInStock { get; set; }
    }
}