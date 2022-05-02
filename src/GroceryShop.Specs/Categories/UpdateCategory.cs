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
using GroceryShop.Infrastructure.Application;

namespace GroceryShop.Specs.Categories
{
    [Scenario("ویرایش دسته بندی")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "   دسته بندی کالا را مدیریت کنم",
        InOrderTo = "آنها را ویرایش کنم"
    )]
    public class UpdateCategory : EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly CategoryServices _sut;
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


        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته ها موجود است")]
        public void Given() 
        {
            Category category = CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }


        [When("دسته بندی با عنوان 'لبنیات' را به 'پروتئینی' ویرایش میکنم")]
        public void When()
        {
            UpdateCategoryDto dto = GenerateUpdateCategoryDto("protoen");
            _sut.Update(dto, 1);

        }

        [Then("دسته بندی با عنوان'لبنیات'در فهرست دسته بندی کالا باید وجود داشته باشد.")]
        public void Then()
        {
            UpdateCategoryDto dto = GenerateUpdateCategoryDto("protoen");

            var expected = _dataContext.Categories
                .FirstOrDefault(_ => _.Id == 1);
            expected.Name.Should().Be(dto.Name);
        }

            [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }

        private static UpdateCategoryDto GenerateUpdateCategoryDto(string name)
        {
            return new UpdateCategoryDto
            {
                Id = 1,
                Name = name,
            };
        }
        private static Category CreateCategory(string name)
        {
            return new Category
            {
                Id =1,
                Name = name,
            };
        }

    }
}
