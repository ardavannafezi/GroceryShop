using GroceryShop.Entities;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Imports.Contract;
using GroceryShop.Services.Sells.Contract;

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
     
        public UpdateSellDto Build()
        {
            return selltDto;
        }
    }
}