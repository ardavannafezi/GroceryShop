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
        private readonly ImportServices _sut;
        private readonly ImportRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Import import;
        Product product;
        Category category;

        public DeleteImport(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFImportRepository(_dataContext);

            _sut = new ImportAppServices(_repository, _unitOfWork);
        }


        [Given("ورودی کالا با کد '01' و1 عدد از 1 کالا در فهرست ورودی کالا ها موجود است")]
        public void Given()
        {
            CreateCategoryInDatabase("labaniyat");
            CreateProductInDatabase("maste shirazi", 1, category.Id, 1);
            CreateImportInDatabase(product.ProductCode , 1);
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
            _dataContext.Products
                .FirstOrDefault(_ => _.ProductCode == import.ProductCode)
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
