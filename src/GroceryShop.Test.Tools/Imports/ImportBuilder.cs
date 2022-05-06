using GroceryShop.Entities;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Imports.Contract;

namespace GroceryShop.TestTools.categories
{
    public class ImportBuilder
    {
        Import import;
        public ImportBuilder()
        {
            import = new Import
            {
                Id = 1,
                ProductCode = 1,
                Price = 100,
                Quantity = 4,
            };
        }
        public ImportBuilder WithProductCode(int code)
        {
            import.ProductCode = code;
            return this;
        }
        public ImportBuilder WithQuantity(int quantity)
        {
            import.Quantity = quantity;
            return this;
        }
        public ImportBuilder WithPrice(double price)
        {
            import.Price = price;
            return this;
        }
        public Import Build()
        {
            return import;
        }
    }
}