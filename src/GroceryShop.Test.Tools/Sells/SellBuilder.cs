using GroceryShop.Entities;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Imports.Contract;

namespace GroceryShop.TestTools.Sells
{
    public class SellBuilder
    {
        Sell sell;
        public SellBuilder()
        {
            sell = new Sell
            {
                ProductCode = 1,
                Quantity = 4,
            };
        }
        public SellBuilder WithProductCode(int code)
        {
            sell.ProductCode = code;
            return this;
        }
        public SellBuilder WithQuantity(int quantity)
        {
            sell.Quantity = quantity;
            return this;
        }
        
        public Sell Build()
        {
            return sell;
        }
    }
}