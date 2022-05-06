﻿using System;
using System.Linq;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Categories;
using GroceryShop.Specs.Infrastructure;
using FluentAssertions;
using Xunit;
using static GroceryShop.Specs.BDDHelper;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Books.Contracts;
using BookStore.Persistence.EF;
using GroceryShop.Services.Categories;
using GroceryShop.Infrastructure.Application;
using GroceryShop.TestTools.categories;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.Persistence.EF.Products;
using GroceryShop.Services.Products;
using GroceryShop.TestTools.Products;

namespace GroceryShop.Specs.Products
{
    [Scenario("حذف کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "    کالا را مدیریت کنم",
        InOrderTo = "آنها را حذف کنم"
    )]
    public class DeleteProductWithNotExistedCode: EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices  _sut;
        private readonly ProductRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private AddCategoryDto _dto;
        Action expected;

        public DeleteProductWithNotExistedCode(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new ProductAppServices(_repository, _unitOfWork, _categoryRepository);
        }

        [Given("کالایی با کد 2 در فهرست کالا وجود ندارد")]
        public void Given()
        {
        }


        [When("کالای با کد '02'  را حذف می کنیم")]
        public void When()
        {
            expected = () => _sut.Delete(2);
        }

        [Then("هیچ کالایی در فهرست کالا ها با کد '02' وجود ندارد  ")]
        public void Then()
        {

            expected.Should().ThrowExactly<ProductNotFoundExeption>();
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
    }
}
