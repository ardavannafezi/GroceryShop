﻿using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Categories;
using GroceryShop.Persistence.EF.Imports;
using GroceryShop.Persistence.EF.Products;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Imports;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.Specs.Infrastructure;
using GroceryShop.TestTools.categories;
using GroceryShop.TestTools.Products;
using System;
using Xunit;
using static GroceryShop.Specs.BDDHelper;

namespace GroceryShop.Specs.BuyProducts
{
    [Scenario("تعریف ورودی کالا")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = "ورودی کالا را مدیریت",
      InOrderTo = "ورودی مشاهده کنم"
  )]
    public class GetImports : EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices _productSut;
        private readonly ImportServices _sut;

        private readonly ProductRepository _productRepository;
        private readonly ImportRepository _repository;

        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private Product _product;
        Action expected;
        Import import;

        public GetImports(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFImportRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);

            _sut = new ImportAppServices(_repository, _unitOfWork, _categoryRepository , _productRepository);
        }


            [Given("کالایی با کد '01' و تعداد' 1' عدد  وارد شده")]
            public void Given()
            {
                var category = CategoryFactory.CreateCategory("labaniyat");
                _dataContext.Manipulate(_ => _.Categories.Add(category));

                int categoryId = _categoryRepository.FindByName(category.Name).Id;
                var product = new ProductFactory()
                   .WithName("maste shirazi")
                   .WithCategoryId(categoryId)
                   .WithProductCode(1)
                   .Build();
                _dataContext.Manipulate(_ => _.Products.Add(product));

                 import = new ImportBuilder()
                    .WithProductCode(1)
                    .WithQuantity(1)
                    .Build();
                _dataContext.Manipulate(_ => _.Imports.Add(import));

            }

        [When("میخواهیم لیست تمامی ورودی ها را دریافت کنیم")]
        public void When()
        {
            _sut.GetAll();

        }

        [Then(" ورودی با کد کالای '01' و تعداد 1' و به ما داده می شود")]
        public void Then()
        {
            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.ProductCode == import.ProductCode
            && _.Quantity == import.Quantity );
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
            , _ => When()
            , _ => Then()) ;
        }
    }
}
