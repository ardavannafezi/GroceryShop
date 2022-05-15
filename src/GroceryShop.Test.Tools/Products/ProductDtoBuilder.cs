using GroceryShop.Services.Categories.Contracts;

namespace GroceryShop.TestTools.categories
{
    public class ProductDtoBuilder
    {
        AddProductDto productDto;
        public ProductDtoBuilder()
        {
            productDto = new AddProductDto
            {
                ProductCode = 1,
                Name = "dummy",
                CategoryId = 1,
                MaxInStock = 5,
                MinInStock = 1,
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
        public ProductDtoBuilder WithCategoryId(int categoryId)
        {
            productDto.CategoryId = categoryId;
            return this;
        }
        public ProductDtoBuilder WithMaxInStock(int maxInStock)
        {
            productDto.MaxInStock = maxInStock;
            return this;
        }
        public ProductDtoBuilder WithMinInStock(int minInStock)
        {
            productDto.MinInStock = minInStock;
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
                CategoryId = 1,
                MaxInStock = 5,
                MinInStock = 1,
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
 
        public UpdateProductDtoBuilder WithCategoryId(int id)
        {
            productDto.CategoryId = id;
            return this;
        }
        public UpdateProductDtoBuilder WithMaxInStock(int max)
        {
            productDto.MaxInStock = max;
            return this;
        }
        public UpdateProductDtoBuilder WithMinInStock(int min)
        {
            productDto.MinInStock = min;
            return this;
        }

        public UpdateProductDto Build()
        {
            return productDto;
        }
    }
}
