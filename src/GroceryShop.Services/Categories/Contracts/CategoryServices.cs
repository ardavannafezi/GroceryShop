using GroceryShop.Infrastructure.Application;
using GroceryShop.Services.Categories.Contracts;
using System.Collections.Generic;

namespace GroceryShop.Services.Books.Contracts
{
    public interface CategoryServices : Service
    {
        void Add(AddCategoryDto dto);
        void Update(UpdateCategoryDto dto, int id);
        IList<GetCategoryDto> GetAll();
        void Delete(int id);
    }
}
