using System;
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
        IWantTo = " دسته بندی کالا را مدیریت کنم",
        InOrderTo = "آنها را مشاهده کنم"
    )]
    public class GetCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryServices  _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category category;
        public Action expected;

        public GetCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان 'لبنیات'در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            CreateCategoryInDatabase("labaniyat");
        }

        [When("لیست دسته بندی رامشاهده کنم")]
        public void When()
        {
            _sut.GetAll();
        }
   
        [Then("فهرستی از دسته بندی ها نشان داده می شود")]
        public void Then()
        {
            var expected = _sut.GetAll();

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

        private void CreateCategoryInDatabase(string name)
        {
            category = CategoryFactory.CreateCategory(name);
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }
    }
}
