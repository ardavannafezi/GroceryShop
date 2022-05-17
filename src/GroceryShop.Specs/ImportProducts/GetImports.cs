using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Imports;
using GroceryShop.Services.Imports;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.Specs.Infrastructure;
using GroceryShop.TestTools.categories;
using GroceryShop.TestTools.Products;
using Xunit;
using static GroceryShop.Specs.BDDHelper;

namespace GroceryShop.Specs.BuyProducts
{
    [Scenario("مشاهده ورودی کالا")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = "ورودی کالا را مدیریت",
      InOrderTo = "ورودی مشاهده کنم"
  )]
    public class GetImports : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly ImportServices _sut;
        private readonly ImportRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Category category;
        Product product;
        Import import;

        public GetImports(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFImportRepository(_dataContext);

            _sut = new ImportAppServices(_repository, _unitOfWork);
        }


            [Given("کالایی با کد '01' و تعداد' 1' عدد  وارد شده")]
            public void Given()
            {
            CreateCategoryInDatabase("labaniyat");
            CreateProductInDatabase("maste shirazi", 1, category.Id, 0);
            CreateImportInDatabase(1, 1);
            }

        [When("میخواهیم لیست تمامی ورودی ها را دریافت کنیم")]
        public void When()
        {
            _sut.GetAll();
        }

        [Then(" ورودی با کد کالای '01' و تعداد 1' و به ما داده می شود")]
        public void Then()
        {
            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.ProductCode == import.ProductCode
            && _.Quantity == import.Quantity );
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
            , _ => When()
            , _ => Then()) ;
        }

        private void CreateCategoryInDatabase(string name)
        {
            category = CategoryFactory.CreateCategory(name);
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }

        private void CreateProductInDatabase
           (string name,
           int productCode,
           int categoryId,
           int quantity
           )
        {
            product = new ProductFactory()
                .WithName(name)
                .WithCategoryId(categoryId)
                .WithProductCode(productCode)
                .WithQuantity(quantity)
                .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));
        }

        private void CreateImportInDatabase(int productCode, int quantity)
        {
            import = new ImportBuilder()
                .WithProductCode(productCode)
                .WithQuantity(quantity)
                .Build();
            _dataContext.Manipulate(_ => _.Imports.Add(import));
        }
    }
}
