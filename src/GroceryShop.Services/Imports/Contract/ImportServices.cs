using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Imports.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Products.Contracts
{
    public interface ImportServices
    {
        public void Add(AddImportDto dto);
        
    }
}
