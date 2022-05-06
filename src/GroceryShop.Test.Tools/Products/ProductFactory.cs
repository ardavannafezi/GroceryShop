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
                MaxInStock = 15,
                MinInStock = 1,
                BuyPrice = 100,
                SellPrice = 200,
                Quantity = 0,
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

        public ProductFactory WithBuyPrice(double price)
        {
            product.BuyPrice = price;
            return this;
        }

        public ProductFactory WithSellPrice(double price)
        {
            product.SellPrice = price;
            return this;
        }

        public ProductFactory WithCategoryId(int id)
        {
            product.CategoryId = id;
            return this;
        }

        public ProductFactory WithMaxInStock(int quantity)
        {
            product.MaxInStock = quantity;
            return this;
        }

        public ProductFactory WithMinInStock(int quantity)
        {
            product.MinInStock = quantity;
            return this;
        }
        public Product Build()
        {
            return product;
        }
    }
}
