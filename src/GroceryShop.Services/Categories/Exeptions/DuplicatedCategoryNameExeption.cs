using System;

namespace GroceryShop.Services.Categories
{
    [Serializable]
    public class DuplicatedCategoryNameExeption : Exception
    {
        public DuplicatedCategoryNameExeption()
        {
        }

     
    }
}