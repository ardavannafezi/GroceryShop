using System;
using System.Linq;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Specs.Infrastructure;
using FluentAssertions;
using Xunit;
using static GroceryShop.Specs.BDDHelper;
using BookStore.Persistence.EF;
using GroceryShop.Infrastructure.Application;
using GroceryShop.TestTools.categories;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.Persistence.EF.Products;
using GroceryShop.Services.Products;
using GroceryShop.TestTools.Products;
using GroceryShop.Services.Categories.Contracts;

namespace GroceryShop.Specs.Products
{
    [Scenario(" ویرایش کالابا نام تکراری")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالا را مدیریت کنم",
        InOrderTo = "آنها را ویرایش کنم"
    )]
    public class UpdateProductWithDuplicatedName: EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices  _sut;
        private readonly ProductRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Action expected;
        Category category;
        Product product;
        UpdateProductDto dto;

        public UpdateProductWithDuplicatedName(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sut = new ProductAppServices(_repository, _unitOfWork);
        }

        [Given("کالایی با عنوان 'ماست شیرازی' و کد 1 در فهرست لبنیات وجود دارد")]
        public void Given()
        {
            CreateCategoryInDatabase("labaniyat");

            CreateProductInDatabase("maste shirazi", 1, category.Id);
        }

        [And(" کالایی با عنوان 'ماست کاله' و کد 2 در فهرست لبنیات وجود دارد")]
        public void And()
        {
            CreateProductInDatabase("maste kaleh", 2, category.Id);
        }

        [When("کالایی با کد 2 را به عنوان 'ماست شیرازی' ویرایش می کنیم")]
        public void When()
        {
            CreateUpdateProductDto("maste shirazi", 2, category.Id, 2, 12);
    
            expected = () => _sut.Update(dto, 2);
        }

        [Then("تنها کالای 1 با عنوان 'ماست شیرازی' باید در  لبنیات وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.Products
                .Count(_ =>
                    _.Name == product.Name
                    && _.CategoryId == product.CategoryId);
            expected.Should().Be(1); 
        }

        [And("")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<ProductNameIsDuplicatedExeption>();
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
            , _ => And()
            , _ => When()
            , _ => Then()
            , _ => ThenAnd()

            ); ;
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

        private void CreateUpdateProductDto(string name,
           int productCode,
           int categoryId,
           int min,
           int max
           )
        {
            dto = new UpdateProductDtoBuilder()
                 .WithName(name)
                 .WithCategoryId(categoryId)
                 .WithProductCode(productCode)
                 .WithMaxInStock(max)
                 .WithMinInStock(min)
                 .Build();
        }
    }
}
