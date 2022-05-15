using System;
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
    [Scenario("مشاهده کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "   کالا را مدیریت کنم",
        InOrderTo = "آنها را مشاهده کنم"
    )]
    public class GetProduct: EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices  _sut;
        private readonly ProductRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Product product;
        Category category;

        public GetProduct(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sut = new ProductAppServices(_repository, _unitOfWork);
        }

        [Given("لایی با عنوان ماست کاله و کد 1 و حداقل تعداد 1 و حداکثر 10 در دسته لبنیات وجود دارد")]
        public void Given()
        {
            CreateCategoryInDatabase("labaniyat");

            CreateProductInDatabase("maste shirazi", 1, category.Id,1,10);
        }

        [When("میخواهیم تمامی کالا های فهرست را مشاهده کنیم")]
        public void When()
        {
            _sut.GetAll();
        }

        [Then(" لایی با عنوان ماست کاله و کد 1 و حداقل تعداد 1 و حداکثر 10 نشان داده می شود")]
        public void Then()
        {
            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Name == product.Name
            && _.ProductCode == product.ProductCode 
            && _.CategoryId == product.CategoryId);
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
    }
}
