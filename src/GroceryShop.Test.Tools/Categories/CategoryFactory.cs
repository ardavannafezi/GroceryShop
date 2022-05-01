using GroceryShop.Entities;

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
    }
}
