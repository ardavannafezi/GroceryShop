using GroceryShop.Entities;
using GroceryShop.Services.Categories.Contracts;

namespace GroceryShop.TestTools.categories
{
    public static class CategoryFactory
    {
        public static Category CreateCategory(string name)
        {
            return new Category
            {
                Name = name
            };
        }
        public static UpdateCategoryDto UpdateCategoryDto(string name)
        {
            return new UpdateCategoryDto
            {
                Name = name
            };
        }
    }
}
