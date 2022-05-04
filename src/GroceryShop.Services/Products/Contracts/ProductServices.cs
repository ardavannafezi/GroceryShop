using GroceryShop.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Products.Contracts
{
    public interface ProductServices
    {
        void Add(AddProductDto product);
        IList<GetProductDto> GetAll();
    }
}
