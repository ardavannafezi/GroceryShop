using System;
using System.Runtime.Serialization;

namespace GroceryShop.Services.Products
{
    [Serializable]
    public class ProductCodeIsDuplicatedExeption : Exception
    {
        public ProductCodeIsDuplicatedExeption()
        {
        }

        public ProductCodeIsDuplicatedExeption(string message) : base(message)
        {
        }

        public ProductCodeIsDuplicatedExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductCodeIsDuplicatedExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}