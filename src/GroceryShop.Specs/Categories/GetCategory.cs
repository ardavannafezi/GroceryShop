using System;
using System.Linq;
using System.Collections.Generic;
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
    [Scenario("مشاهده دسته بندی کالای")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "   دسته بندی کالا را مدیریت کنم",
        InOrderTo = "آنها را مشاهده کنم"
    )]
    public class GetCategory : EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly CategoryServices  _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private AddCategoryDto _dto;
        public Action expected;

        public GetCategory(ConfigurationFixture configuration) : base(configuration)
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


        [When("درخواست مشاهده فهرست کالا را میدهم")]
        public void When()
        {
            _sut.GetAll();
        }
   
        [Then("فهرستی با عنوان 'لبنیات' به ما نشان داده شود")]
        public void Then()
        {
            var expected = _sut.GetAll();
            var category = CategoryFactory.CreateCategory("labaniyat");
            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Name == category.Name);

        }



        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
            , _ => When()
            , _ => Then());
        }

        private static Category CreateCategoryWithNameLabaniyat()
        {
            return new Category
            {
                Name = "labaniyat"
            };
        }

    }
}
