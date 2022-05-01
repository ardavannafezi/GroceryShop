using GroceryShop.Infrastructure.Application;
using GroceryShop.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Books.Contracts
{
    public interface CategoryServices : Service
    {
        void Add(AddCategoryDto dto);
    }
}
