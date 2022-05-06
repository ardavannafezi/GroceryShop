using System;
using System.Runtime.Serialization;

namespace GroceryShop.Services.Imports
{
    [Serializable]
    public class SellNotFoundExeption : Exception
    {
        public SellNotFoundExeption()
        {
        }

    }
}