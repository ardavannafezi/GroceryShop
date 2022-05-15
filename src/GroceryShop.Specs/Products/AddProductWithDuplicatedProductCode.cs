using System;
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
using GroceryShop.TestTools.Products;

namespace GroceryShop.Specs.Products
{
    [Scenario(" تعریف کالا با کد تکراری")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "   دسته بندی کالا را مدیریت کنم",
        InOrderTo = "آنها را تعریف کنم"
    )]
    public class AddProductWithDuplicatedProductCode : EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices  _sut;
        private readonly ProductRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Category category;
        AddProductDto dto;
        Action expected;
        Product product;

        public AddProductWithDuplicatedProductCode(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sut = new ProductAppServices(_repository, _unitOfWork);
        }

        [Given("کالایی با عنوان 'ماست شیرازی' و کد 2 در فهرست کالا وجود دارد")]
        public void Given()
        {
            CreateCategoryInDatabase("labaniyat");

            CreateProductInDatabase("maste shirazi", 2, category.Id);
        }

        [When("کالایی با عنوان 'ماست کاله' با کد 2 تعریف میکنم")]
        public void When()
        {
            dto = CreateProductDtoWithBuilder
               ("maste kaleh", 2, category.Id);

            expected = () => _sut.Add(dto);
        }

        [Then("تنها یک کالا با کد '2' باید در فهرست کالا وجود داشته باشد  ")]
        public void Then()
        {
            var expected = _dataContext.Products
                .Count(_ => _.ProductCode == product.ProductCode);
            expected.Should().Be(1);
            
        }

        [And("خطایی با عنوان ' کد کالا تکراریست ' باید رخ دهد.")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<ProductCodeIsDuplicatedExeption>();
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
              _ => Given()
            , _ => When()
            , _ => Then()
            , _ => ThenAnd());;
        }

        private void CreateCategoryInDatabase(string name)
        {
            category = CategoryFactory.CreateCategory(name);
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }

        private void CreateProductInDatabase
            (string name,
            int productCode,
            int categoryId
            )
        {
            product = new ProductFactory()
                .WithName(name)
                .WithCategoryId(categoryId)
                .WithProductCode(productCode)
                .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));
        }

        private static AddProductDto CreateProductDtoWithBuilder
            (string name,
             int productCode,
             int categoryId
            )
        {
            return new ProductDtoBuilder()
               .WithName(name)
               .WithProductCode(productCode)
               .WithCategoryId(categoryId)
               .Build();
        }
    }
}
