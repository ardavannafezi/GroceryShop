using System;
using System.Runtime.Serialization;

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