using System.Linq;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Specs.Infrastructure;
using FluentAssertions;
using Xunit;
using static GroceryShop.Specs.BDDHelper;
using GroceryShop.Services.Categories.Contracts;
using BookStore.Persistence.EF;
using GroceryShop.Infrastructure.Application;
using GroceryShop.TestTools.categories;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.Persistence.EF.Products;
using GroceryShop.Services.Products;

namespace GroceryShop.Specs.Products
{
    [Scenario("تعریف کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "کالا را مدیریت کنم",
        InOrderTo = "آنها را تعریف کنم"
    )]
    public class AddProduct: EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly ProductServices  _sut;
        private readonly ProductRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Category category;
        AddProductDto dto;

        public AddProduct(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sut = new ProductAppServices(_repository, _unitOfWork);
        }

        [Given("هیچ کالایی در فهرست کالا وجود ندارد")]
        public void Given()
        {
            _dataContext.Products.Any().Should().BeFalse();
        }

        [When("کالایی با عنوان ماست کاله و کد 1 و حداقل تعداد 1 و حداکثر 10 تعریف می کنیم")]
        public void When()
        {
            CreateCategoryInDatabase("labaniyat");

            dto = CreateProductDtoWithBuilder
                ("maste kaleh", 1 ,category.Id, 1, 10);

            _sut.Add(dto);
        }

        [Then("کالایی با عنوان ماست کاله و کد 1 و حداقل تعداد 1 و حداکثر 10 باید وجود داشته باشد ")]
        public void Then()
        {
            var expected = _dataContext.Products
                .FirstOrDefault(_ => _.Name == dto.Name 
                &&  _.ProductCode == dto.ProductCode 
                && _.MinInStock == dto.MinInStock
                && _.MaxInStock == dto.MaxInStock);
            expected.Should().NotBeNull();
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
              _ => Given()
            , _ => When()
            , _ => Then());
        }

        private void CreateCategoryInDatabase(string name)
        {
            category = CategoryFactory.CreateCategory(name);
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }

        private static AddProductDto CreateProductDtoWithBuilder
            (string name,
            int productCode,
            int categoryId,
            int min,
            int max
            )
        {
            return new ProductDtoBuilder()
               .WithName(name)
               .WithProductCode(productCode)
               .WithCategoryId(categoryId)
               .WithMinInStock(min)
               .WithMaxInStock(max)
               .Build();
        }
    }
}
