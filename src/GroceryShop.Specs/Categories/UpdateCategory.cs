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

namespace GroceryShop.Specs.Categories
{
    [Scenario("تعریف دسته بندی")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "   دسته بندی کالا را مدیریت کنم",
        InOrderTo = "آنها را تعریف کنم"
    )]
    public class UpdateCategory: EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly CategoryServices  _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private AddCategoryDto _dto;
        Action expected;

        public UpdateCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork, _categoryRepository);
        }

        [Given("دسته بندی با عنوان 'لبنیات'در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }


        [When("دسته بندی با عنوان 'لبنیات' را به 'پروتئینی' ویرایش میکنم")]
        public void When()
        {
           var categoryDto = CategoryFactory.UpdateCategoryDto("Protoeny");

           _sut.Update(categoryDto, "labaniyat");
        }

        [Then("دسته بندی با عنوان 'پروتئین' در فهرست دسته بندی کالا باید وجود داشته باشد ")]
        public void Then()
        {

            var expected = _dataContext.Categories.FirstOrDefault();
            expected.Name.Should().Be(CategoryFactory.CreateCategory("Protoeny").Name);

        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
            , _ => When()
            , _ => Then());
        }




       
    }
}