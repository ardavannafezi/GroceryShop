using System;
using System.Runtime.Serialization;

namespace GroceryShop.Services.Imports
{
    [Serializable]
    public class ImportNotFoundExeption : Exception
    {
        public ImportNotFoundExeption()
        {
        }

        public ImportNotFoundExeption(string message) : base(message)
        {
        }

        public ImportNotFoundExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ImportNotFoundExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}