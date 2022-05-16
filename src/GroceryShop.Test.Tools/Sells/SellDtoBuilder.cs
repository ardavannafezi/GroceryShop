using GroceryShop.Services.Sells.Contract;
using System;

namespace GroceryShop.TestTools.categories
{
    public class SellDtoBuilder
    {
        AddSellDto selltDto;
        public SellDtoBuilder()
        {
            selltDto = new AddSellDto
            {
                ProductCode = 1,
                Quantity = 4,
                dateTime = DateTime.Now
                
            };
        }
        public SellDtoBuilder WithProductCode(int code)
        {
            selltDto.ProductCode = code;
            return this;
        }
        public SellDtoBuilder WithQuantity(int quantity)
        {
            selltDto.Quantity = quantity;
            return this;
        }
        public SellDtoBuilder WithDateTime(DateTime dateTime)
        {
            selltDto.dateTime = dateTime;
            return this;
        }

        public AddSellDto Build()
        {
            return selltDto;
        }
    }
    public class UpdatSellDtoBuilder
    {
        UpdateSellDto selltDto;
        public UpdatSellDtoBuilder()
        {
            selltDto = new UpdateSellDto
            {
                ProductCode = 1,
                Quantity = 4,
                dateTime = DateTime.Now
                
            };
        }
        public UpdatSellDtoBuilder WithProductCode(int code)
        {
            selltDto.ProductCode = code;
            return this;
        }
        public UpdatSellDtoBuilder WithQuantity(int quantity)
        {
            selltDto.Quantity = quantity;
            return this;
        }

        public UpdatSellDtoBuilder WithDateTime(DateTime dateTime)
        {
            selltDto.dateTime = dateTime;
            return this;
        }

        public UpdateSellDto Build()
        {
            return selltDto;
        }
    }
}