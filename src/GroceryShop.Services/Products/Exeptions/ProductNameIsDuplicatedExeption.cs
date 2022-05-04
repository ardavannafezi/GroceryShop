using System;
using System.Runtime.Serialization;

namespace GroceryShop.Services.Products
{
    [Serializable]
    public class ProductNameIsDuplicatedExeption : Exception
    {
        public ProductNameIsDuplicatedExeption()
        {
        }

        public ProductNameIsDuplicatedExeption(string message) : base(message)
        {
        }

        public ProductNameIsDuplicatedExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductNameIsDuplicatedExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}