using System;
using System.Runtime.Serialization;

namespace GroceryShop.Services.Categories
{
    [Serializable]
    public class CategoryHasExistingProduct : Exception
    {
        public CategoryHasExistingProduct()
        {
        }

        public CategoryHasExistingProduct(string message) : base(message)
        {
        }

        public CategoryHasExistingProduct(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CategoryHasExistingProduct(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}