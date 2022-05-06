using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Sells.Contract
{
    public class AddSellDto
    {
        public int ProductCode { get; set; }
        public int Quantity { get; set; }
    }
}
