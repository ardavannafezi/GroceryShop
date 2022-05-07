using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Categories;
using GroceryShop.Specs.Infrastructure;
using FluentAssertions;
using Xunit;
using static GroceryShop.Specs.BDDHelper;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Persistence.EF;
using GroceryShop.Services.Books.Contracts;
using BookStore.Persistence.EF;
using GroceryShop.Services.Categories;

namespace GroceryShop.Specs.Categories
{
    [Scenario("تعریف دسته بندی")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "   دسته بندی کالا را مدیریت کنم",
        InOrderTo = "آنها را تعریف کنم"
    )]
    public class AddCategories : EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;

        private Category _category;
        private AddCategoryDto _dto;
        public AddCategories(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
        }

        [Given("هیچ دسته بندی در فهرست دسته بندی کالا وجود ندارد")]
        public void Given() 
        {

        }

        [When("دسته بندی با عنوان 'لبنیات' تعریف میکنم")]
        public void When()
        {
            _dto = new AddCategoryDto()
            {
                Name =  "labaniyat"
            };
            var _unitOfWork = new EFUnitOfWork(_dataContext);
            CategoryRepository _categoryRepository = new EFCategoryRepository(_dataContext);
            CategoryServices _sut = new CategoryAppService(_categoryRepository,
                _unitOfWork,
                _productrepository);

            _sut.Add(_dto);
        }

        [Then("دسته بندی با عنوان'لبنیات'در فهرست دسته بندی کالا باید وجود داشته باشد.")]
        public void Then()
        {
            var expected = _dataContext.Categories.FirstOrDefault();
            expected.Name.Should().Be(_dto.Name);
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }


    }
}
