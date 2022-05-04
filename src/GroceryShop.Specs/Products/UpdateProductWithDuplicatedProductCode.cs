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
using GroceryShop.Services.Products.Contracts;
using GroceryShop.Persistence.EF.Products;
using GroceryShop.Services.Products;
using GroceryShop.TestTools.Products;

namespace GroceryShop.Specs.Categories
{
    [Scenario("ویرایش کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "   دسته بندی کالا را مدیریت کنم",
        InOrderTo = "آنها را ویرایش کنم"
    )]
    public class UpdateProductWithDuplicatedProductCode: EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices  _sut;
        private readonly ProductRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private AddCategoryDto _dto;
        Action expected;

        public UpdateProductWithDuplicatedProductCode(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new ProductAppServices(_repository, _unitOfWork, _categoryRepository);
        }

        [Given("کالایی با عنوان 'ماست شیرازی' و کد 2 در فهرست کالا وجود دارد")]
        public void Given()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            int categoryId = _categoryRepository.FindByName(category.Name).Id;
            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(categoryId)
               .WithProductCode(2)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));
        }

        [And(" کالایی با عنوان 'ماست کاله' و کد 3در فهرست کالا وجود دارد")]
        public void And()
        {

            int categoryId = _categoryRepository.FindByName("labaniyat").Id;
            var product = new ProductFactory()
               .WithName("maste kaleh")
               .WithCategoryId(categoryId)
               .WithProductCode(3)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));
        }

        [When("کالایی با کد 03 ر به  کد 2 ویرایش می کنیم")]
        public void When()
        {
            var dto = new UpdateProductDtoBuilder()
               .WithName("maste kaleh")
               .WithCategoryName("labaniyat")
               .WithProductCode(2)
               .Build();

            expected = () => _sut.Update(dto, 3);

        }

        [Then("تنها کالای 02 با عنوان 'ماست شیرازی' باید در کالا وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.Products.Any(_ => _.ProductCode == 2 && _.Name == "maste shirazi");
            expected.Should().BeTrue();
            
        }

        [And("خطایی با عنوان 'کد جدید کالا تکراریست ' باید رخ دهد")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<ProductCodeIsDuplicatedExeption>();

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

            ); ;
        }      
    }
}
