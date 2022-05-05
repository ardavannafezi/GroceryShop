using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Categories.Contracts
{
    public class AddProductDto
    {
        public int ProductCode { get; set; }

        public string Name { get; set; }
        public string CategoryName { get; set; }
        public double BuyPrice { get; set; }
        public double SellPrice { get; set; }
        public int Quantity { get; set; }
        public int MaxInStock { get; set; }
        public int MinInStock { get; set; }
    }
}
