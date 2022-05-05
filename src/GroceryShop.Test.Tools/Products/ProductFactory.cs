using GroceryShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.TestTools.Products
{

    public class ProductFactory
    {
        Product product;
        public ProductFactory()
        {
            product = new Product
            {
                ProductCode = 1,
                Name = "dummy",
                CategoryId = 1,
                MaxInStock = 5,
                MinInStock = 1,
                BuyPrice = 100,
                SellPrice = 200,
                Quantity = 4,
            };
        }
        public ProductFactory WithProductCode(int code)
        {
            product.ProductCode = code;
            return this;
        }
        public ProductFactory WithName(string name)
        {
            product.Name = name;
            return this;
        }
        public ProductFactory WithQuantity(int numbers)
        {
            product.Quantity = numbers;
            return this;
        }
        public ProductFactory WithCategoryId(int id)
        {
            product.CategoryId = id;
            return this;
        }
        public Product Build()
        {
            return product;
        }
    }
}
