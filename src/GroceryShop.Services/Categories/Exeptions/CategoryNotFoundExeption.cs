using System;
using System.Runtime.Serialization;

namespace GroceryShop.Services.Categories
{
    [Serializable]
    public class CategoryNotFoundExeption : Exception
    {
        public CategoryNotFoundExeption()
        {
        }

     
    }
}