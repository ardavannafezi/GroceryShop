using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Categories;
using GroceryShop.Services.Books.Contracts;
using GroceryShop.Services.Categories;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Specs.Infrastructure;
using GroceryShop.TestTools.categories;
using System.Linq;
using Xunit;
using static GroceryShop.Specs.BDDHelper;

namespace GroceryShop.Specs.Categories
{
    [Scenario("تعریف دسته بندی کالا")]
    [Feature("",
        AsA = "فروشنده",
        IWantTo = "دسته بندی کالا را مدیریت کنم",
        InOrderTo = "آنها را تعریف کنم"
    )]
    public class AddCategories : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryServices _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        AddCategoryDto dto;

        public AddCategories(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Given("هیچ دسته بندی در فهرست دسته بندی کالا وجود ندارد")]
        public void Given()
        {
            _dataContext.Categories.Any().Should().BeFalse();
        }

        [When("دسته بندی با عنوان 'لبنیات' تعریف میکنم")]
        public void When()
        {
            dto = CategoryFactory.AddCategoryDto("labaniyat");
            _sut.Add(dto);
        }

        [Then("دسته بندی با عنوان'لبنیات'در فهرست دسته بندی کالا موجود است.")]
        public void Then()
        {
            var expected = _dataContext.Categories
                .Any(_ => _.Name == dto.Name).Should().BeTrue();
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

