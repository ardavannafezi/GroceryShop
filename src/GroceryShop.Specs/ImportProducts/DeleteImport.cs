using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Categories;
using GroceryShop.Persistence.EF.Imports;
using GroceryShop.Persistence.EF.Products;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Imports;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.Specs.Infrastructure;
using GroceryShop.TestTools.categories;
using GroceryShop.TestTools.Products;
using System;
using System.Linq;
using Xunit;
using static GroceryShop.Specs.BDDHelper;

namespace GroceryShop.Specs.BuyProducts
{
    [Scenario("تعریف ورودی کالا")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = "ورودی کالا را مدیریت",
      InOrderTo = "ورودی حذف کنم"
  )]
    public class DeleteImport : EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices _productSut;
        private readonly ImportServices _sut;

        private readonly ProductRepository _productRepository;
        private readonly ImportRepository _repository;

        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private Product _product;
        private Import import;
        
        Action expected;

        public DeleteImport(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFImportRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);

            _sut = new ImportAppServices(_repository, _unitOfWork, _categoryRepository , _productRepository);
        }


        [Given("ورودی کالا با کد '01' و1 عدد از 1 کالا در فهرست ورودی کالا ها موجود است")]
        public void Given()
        {

            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            int categoryId = _categoryRepository.FindByName(category.Name).Id;
            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(categoryId)
               .WithProductCode(1)
               .WithQuantity(1)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));

            import = new ImportBuilder()
                .WithProductCode(1)
                .WithQuantity(1)
                .Build();
            _dataContext.Manipulate(_ => _.Imports.Add(import));
            
        }

        [When("ورودی کالای کد '01' را حذف می کنیم")]
        public void When()
        {
            _sut.Delete(import.Id);
        }

        public void Then()
        {
            _dataContext.Imports.FirstOrDefault(_ => _.Id == import.Id)
                .Should().BeNull();
        }

        [And(" هیچ کالا یی موجود نمی باشد")]

        public void ThenAnd()
        {
            int productCode = _productRepository.FindById(import.ProductCode).ProductCode;
            _dataContext.Products
                .FirstOrDefault(_ => _.ProductCode == productCode)
                .Quantity.Should().Be(0);
        }


        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
            , _ => When()
            , _ => Then()
            , _ => ThenAnd()
            );
        }
    }
}
