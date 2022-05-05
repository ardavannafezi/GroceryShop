using System;
using System.Runtime.Serialization;

namespace GroceryShop.Services.Imports
{
    [Serializable]
    public class ReachedMaximumAllowedInStockExeption : Exception
    {
        public ReachedMaximumAllowedInStockExeption()
        {
        }

        public ReachedMaximumAllowedInStockExeption(string message) : base(message)
        {
        }

        public ReachedMaximumAllowedInStockExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReachedMaximumAllowedInStockExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}