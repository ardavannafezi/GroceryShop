using GroceryShop.Entities;
using System.Collections.Generic;

namespace GroceryShop.Services.Categories.Contracts
{
    public interface CategoryRepository
    {
        public void Add(Category category);
        bool IsCategoryExistById(string name);
        void Update(Category category);
        IList<GetCategoryDto> GetAll();
        Category FindByName(string name);
        Category FindById(int id);
        bool IsCategoryExist(int id);
        bool IsCategoryByName(string Name);
        void Delete(Category category);
        bool isHavingProduct(int categoryId);
    }
}
