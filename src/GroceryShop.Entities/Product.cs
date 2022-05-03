using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public double BuyPrice { get; set; }
        public double SellPrice { get; set; }
        public int Quantity { get; set; }
        public int? MaxInStock { get; set; }
        public int? MinInStock { get; set; }


        public Category Category { get; set; }
    }
}
