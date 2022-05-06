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
                ProductCode = 1,
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
        
        public Import Build()
        {
            return import;
        }
    }
}