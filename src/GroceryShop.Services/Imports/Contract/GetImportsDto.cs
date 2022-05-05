using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Imports.Contract
{
    public class GetImport
    {
        public int Id { get; set; }
        public int ProductCode { get; set; }
        public int Quantity { get; set; }
        public Double Price { get; set; }
    }
}
