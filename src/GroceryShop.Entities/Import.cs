namespace GroceryShop.Entities
{
    public class Import
    {
        public int Id { get; set; }
        public int ProductCode { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
    }
}
