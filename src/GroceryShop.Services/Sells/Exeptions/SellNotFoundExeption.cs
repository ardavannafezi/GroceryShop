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

        public SellNotFoundExeption(string message) : base(message)
        {
        }

        public SellNotFoundExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SellNotFoundExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}