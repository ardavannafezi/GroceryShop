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

        public ReachedMinimumInStockAlert(string message) : base(message)
        {
        }

        public ReachedMinimumInStockAlert(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReachedMinimumInStockAlert(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}