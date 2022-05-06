using System;
using System.Runtime.Serialization;

namespace GroceryShop.Services.Imports
{
    [Serializable]
    public class NotEcoughtInStock : Exception
    {
        public NotEcoughtInStock()
        {
        }

        public NotEcoughtInStock(string message) : base(message)
        {
        }

        public NotEcoughtInStock(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotEcoughtInStock(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}