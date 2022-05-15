using System;
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
    [Scenario(" ویرایش دسته بندی با عنوان تکراری")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "   دسته بندی کالا را مدیریت کنم",
        InOrderTo = "آنها را ویرایش کنم"
    )]
    public class UpdateCategoryWithDuplicatedName: EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryServices  _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Action expected;
        Category category;
        UpdateCategoryDto dto;

        public UpdateCategoryWithDuplicatedName(ConfigurationFixture configuration) : base(configuration)
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

        [And(" 'پروتئینی 'در فهرست دسته بندی کالا وجود دارد")]
        public void And()
        {
            CreateCategoryInDatabase("Protoeny");
        }

        [When("دسته بندی با عنوان 'پروتئینی' را به 'لبنیات' ویرایش میکنیم")]
        public void When()
        {
            dto = CategoryFactory.UpdateCategoryDto("Protoeny");

            expected = () => _sut.Update(dto, category.Id);
        }

        [Then("تنها یک دسته بندی با عنوان ' لبنیات' باید در فهرست دسته بندی کالا وجود داشته  ")]
        public void Then()
        {
            var expected = _dataContext.Categories
                .Any(_ => _.Name == category.Name);
        }

        [And("عنوان 'عنوان جدید دسته بندی کالای تکراریست ' باید رخ دهد.")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<TheCategoryNameAlreadyExist>();
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
            );
        }
        private void CreateCategoryInDatabase(string name)
        {
            category = CategoryFactory.CreateCategory(name);
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }
    }
}
