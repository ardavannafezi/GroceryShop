using System;
using System.Runtime.Serialization;

namespace GroceryShop.Services.Products
{
    [Serializable]
    public class ProductNotFoundExeption : Exception
    {
        public ProductNotFoundExeption()
        {
        }

        public ProductNotFoundExeption(string message) : base(message)
        {
        }

        public ProductNotFoundExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductNotFoundExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}