using System;
using System.Runtime.Serialization;

namespace GroceryShop.Services.Imports
{
    [Serializable]
    internal class ReachedMinimumInStockAlert : Exception
    {
        public ReachedMinimumInStockAlert()
        {
        }

       
    }
}