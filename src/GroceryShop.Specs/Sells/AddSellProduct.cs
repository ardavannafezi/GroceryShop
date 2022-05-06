﻿using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Categories;
using GroceryShop.Persistence.EF.Imports;
using GroceryShop.Persistence.EF.Products;
using GroceryShop.Persistence.EF.Sells;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Imports;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.Services.Sells.Contracts;
using GroceryShop.Specs.Infrastructure;
using GroceryShop.TestTools.categories;
using GroceryShop.TestTools.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static GroceryShop.Specs.BDDHelper;

namespace GroceryShop.Specs.SellProducts
{
    [Scenario("تعریف ورودی کالا")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = "ورودی کالا را مدیریت",
      InOrderTo = "ورودی تعریف کنم"
  )]
    public class AddSellProduct : EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices _productSut;
        private readonly SellServices _sut;

        private readonly ProductRepository _productRepository;
        private readonly SellRepository _repository;

        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private Product _product;
        Action expected;

        public AddSellProduct(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFSellRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);

            _sut = new SellAppServices(_repository, _unitOfWork, _categoryRepository , _productRepository);
        }


        [Given("الایی با کد '01' در فهرست کالا ها تعریف شده است و  6 عدد موجود است")]
        public void Given()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            int categoryId = _categoryRepository.FindByName(category.Name).Id;
            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(categoryId)
               .WithProductCode(1)
               .WithQuantity(6)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));

        }

        [When("کالای 01 را در به تعداد 2 می فروشیم")]
        public void When()
        {
            var dto = new SellDtoBuilder()
              .WithProductCode(1)
              .WithQuantity(2)
              .Build();

            _sut.Add(dto);

        }
        [Then("فروش کالا در لیست فروش موجود است")]

        public void Then()
        {
          _dataContext.Sells.Count(_ => _.ProductCode == 1 && _.Quantity == 2);

        }

        [When("تعداد 4 کالا در لیست کالا ها باقی")]

        public void ThenAnd()
        {
            _productRepository.GetQuantity(1).Should().Be(4);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
            , _ => When()
            , _ => Then()
            , _ => ThenAnd());
      
        }
    }
}