using GroceryShop.Entities;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Imports.Contract;

namespace GroceryShop.TestTools.categories
{
    public class ImportDtoBuilder
    {
        AddImportDto importDto;
        public ImportDtoBuilder()
        {
            importDto = new AddImportDto
            {
                ProductCode = 1,
                Price = 100,
                Quantity = 4,
            };
        }
        public ImportDtoBuilder WithProductCode(int code)
        {
            importDto.ProductCode = code;
            return this;
        }
        public ImportDtoBuilder WithQuantity(int quantity)
        {
            importDto.Quantity = quantity;
            return this;
        }
        public ImportDtoBuilder WithPrice(double price)
        {
            importDto.Price = price;
            return this;
        }
        public AddImportDto Build()
        {
            return importDto;
        }
    }
}