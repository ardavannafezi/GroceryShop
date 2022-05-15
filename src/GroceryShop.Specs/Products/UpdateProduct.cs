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
    [Scenario("ویرایش کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالا را مدیریت کنم",
        InOrderTo = "آنها را ویرایش کنم"
    )]
    public class UpdateProduct: EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices  _sut;
        private readonly ProductRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Category category;
        Action expected;
        UpdateProductDto dto;
        Product product;

        public UpdateProduct(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sut = new ProductAppServices(_repository, _unitOfWork);
        }

        [Given("کالایی با عنوان ماست شیرازی و کد 1 و حداقل تعداد 1 و حداکثر 10 در دسته لبنیات وجود دارد")]
        public void Given()
        {
            CreateCategoryInDatabase("labaniyat");

            CreateProductInDatabase("maste shirazi", 1, category.Id, 1, 10);
        }

        [When("کالایی با کد 1 را به عنوان ماست کاله و حداقل تعداد 2 و حداکثر 12 ویرایش می کنیم")]
        public void When()
        {
            CreateUpdateProductDto("maste kaleh", 1,category.Id ,2,12);

            _sut.Update(dto, 1);
        }

        [Then("کالای کد 1 با عنوان 'ماست کاله' حداقل تعداد 2 و حداکثر 12 در فهرست کالا موجود می باشد")]
        public void Then()
        {
            var expected = _dataContext.Products
                .Any(_ => _.ProductCode == dto.ProductCode
                    && _.Name == dto.Name
                    && _.MaxInStock ==dto.MaxInStock
                    && _.MinInStock == dto.MinInStock);
            expected.Should().BeTrue(); 
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
            , _ => When()
            , _ => Then()
            );;
        }

        private void CreateCategoryInDatabase(string name)
        {
            category = CategoryFactory.CreateCategory(name);
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }

        private void CreateProductInDatabase
            (string name,
            int productCode,
            int categoryId,
            int min,
            int max
            )
        {
            product = new ProductFactory()
                .WithName(name)
                .WithCategoryId(categoryId)
                .WithProductCode(productCode)
                .WithMaxInStock(max)
                .WithMinInStock(min)
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
