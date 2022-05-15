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
    [Scenario(" تعریف کالا با عنوان تکراری")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "    کالا را مدیریت کنم",
        InOrderTo = "آنها را تعریف کنم"
    )]
    public class AddProductWithDuplicatedName : EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices _sut;
        private readonly ProductRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Action expected;
        AddProductDto dto;
        Product product;
        Category category;

        public AddProductWithDuplicatedName(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sut = new ProductAppServices(_repository, _unitOfWork);
        }

        [Given("کالایی با عنوان 'ماست شیرازی' و کد 1 در دسته لبنیات وجود دارد")]
        public void Given()
        {
            CreateCategoryInDatabase("labaniyat");
            CreateProductInDatabase("maste shirazi",1,category.Id);
        }

        [When("کالایی با عنوان 'ماست شیرازی' و کد 3 در دسته لبنیات تعریف میکنم")]
        public void When()
        {
            dto = CreateProductDtoWithBuilder
                ("maste shirazi", 3, category.Id);
            expected = () => _sut.Add(dto);
        }

        [Then("تنها یک کالا با عنوان ' ماست شیرازی' باید در فهرست لبنیات وجود داشته باشد ")]
        public void Then()
        {
            var expected = _dataContext.Products
                .Count(_ => _.Name == product.Name
                && _.CategoryId == product.CategoryId);
            expected.Should().Be(1);
        }

        [And("خطایی با عنوان 'عنوان کالا تکراریست ' باید رخ دهد.")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<ProductNameIsDuplicatedExeption>();
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
            , _ => When()
            , _ => Then()
            , _ => ThenAnd()); ;
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
