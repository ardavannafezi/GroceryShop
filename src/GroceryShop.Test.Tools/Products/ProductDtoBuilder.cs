using GroceryShop.Entities;
using GroceryShop.Services.Categories.Contracts;

namespace GroceryShop.TestTools.categories
{
    public class ProductDtoBuilder
    {
        AddProductDto productDto;
        public ProductDtoBuilder()
        {
           productDto =  new AddProductDto
            {
                ProductCode = 1,
                Name = "dummy",
                CategoryName = "dummy",
                MaxInStock = 5,
                MinInStock = 1,
                BuyPrice = 100,
                SellPrice = 200,
                Quantity = 4,
            };
        }
        public ProductDtoBuilder WithProductCode(int code)
        {
            productDto.ProductCode = code;
            return this;
        }
        public ProductDtoBuilder WithName(string name)
        {
            productDto.Name = name;
            return this;
        }
        public ProductDtoBuilder WithCategoryName(string name)
        {
            productDto.CategoryName = name;
            return this;
        }
        public AddProductDto Build()
        {
            return productDto;
        }
    }
    public class UpdateProductDtoBuilder
    {
        UpdateProductDto productDto;
        public UpdateProductDtoBuilder()
        {
            productDto = new UpdateProductDto
            {
                ProductCode = 1,
                Name = "dummy",
                CategoryName = "dummy",
                MaxInStock = 5,
                MinInStock = 1,
                BuyPrice = 100,
                SellPrice = 200,
                Quantity = 4,
            };
        }
        public UpdateProductDtoBuilder WithProductCode(int code)
        {
            productDto.ProductCode = code;
            return this;
        }
        public UpdateProductDtoBuilder WithName(string name)
        {
            productDto.Name = name;
            return this;
        }
        public UpdateProductDtoBuilder WithCategoryName(string name)
        {
            productDto.CategoryName = name;
            return this;
        }
        public UpdateProductDto Build()
        {
            return productDto;
        }
    }
}
