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
      InOrderTo = "ورودی ویرایش کنم"
  )]
    public class UpdateImport : EFDataContextDatabaseFixture
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
        Action expected;
        private Import import;

        public UpdateImport(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFImportRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);

            _sut = new ImportAppServices(_repository, _unitOfWork, _categoryRepository , _productRepository);
        }


        [Given("کالایی با کد '01' و تعداد' 3'  وارد شده")]
        public void Given()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            int categoryId = _categoryRepository.FindByName(category.Name).Id;
            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(categoryId)
               .WithQuantity(8)
               .WithProductCode(1)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));

            import = new ImportBuilder()
                .WithProductCode(1)
                .WithQuantity(3)
                .Build();
            _dataContext.Manipulate(_ => _.Imports.Add(import));

        }

        [When("ورودی کالا با کد 1 را به تعداد 5  ویرایش می کنم")]
        public void When()
        {

            var dto = new UpdateImportDtoBuilder()
                .WithProductCode(1)
                .WithQuantity(5)
                .Build();

            _sut.Update(dto ,import.Id);

        }

        [Then(" ورودی با کد کالای '01' و تعداد 1' و موجود میباشد")]
        public void Then()
        {
            _dataContext.Imports.Count(_ => _.ProductCode == 1
            && _.Quantity == 5 ).Should().Be(1);
        }

        [And(" تعداد 1عدد از کالا موجود می باشد")]
        public void ThenAnd()
        {
            int productCode = _productRepository.FindById(import.ProductCode).ProductCode;
            _dataContext.Products
                .FirstOrDefault(_ => _.ProductCode == productCode)
                .Quantity.Should().Be(10);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
            , _ => When()
            , _ => Then()
            , _ => ThenAnd()) ;
        }
    }
}
