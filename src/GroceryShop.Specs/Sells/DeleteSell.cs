using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Sells;
using GroceryShop.Services.Imports;
using GroceryShop.Services.Sells.Contracts;
using GroceryShop.Specs.Infrastructure;
using GroceryShop.TestTools.categories;
using GroceryShop.TestTools.Products;
using GroceryShop.TestTools.Sells;
using System;
using System.Linq;
using Xunit;
using static GroceryShop.Specs.BDDHelper;

namespace GroceryShop.Specs.SellProducts
{
    [Scenario("حذف فروش کالا")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = "فروش کالا را مدیریت",
      InOrderTo = "فروش کالا را حذف کنم"
  )]
    public class DeleteSell : EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly SellServices _sut;
        private readonly SellRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Sell sell;
        Category category;
        Product product;
        int LastQuantity;
        public DeleteSell(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFSellRepository(_dataContext);

            _sut = new SellAppServices(_repository, _unitOfWork);
        }

        [Given("فروش کالای با کد 1 از کالایی با کد 1 و تعداد 1 از 6 تا موجودی کالا در فهرست فروش کالا ها موجود است")]
        public void Given()
        {
            CreateCategoryInDatabase("labaniyat");
            CreateProductInDatabase("maste shirazi", 1, category.Id, 6);
            LastQuantity = product.Quantity;
            CreateSellInDatabase( 1 , 1 , DateTime.Now);
        }

        [When("فروش کد 1 را حذف می کنیم")]
        public void When()
        {
            _sut.Delete(sell.Id);
        }

        [Then("هیچ فروشی با کد 1 وجود ندارد")]
        public void Then()
        {
            _dataContext.Imports.FirstOrDefault(_ => _.Id == sell.Id)
                .Should().BeNull();
        }

        [And(" تعداد 7 عدد از کالا موجود می باشد")]
        public void ThenAnd()
        {
            int QuantityAfterDelete = LastQuantity + sell.Quantity;

            _dataContext.Products
                .FirstOrDefault(_ => _.ProductCode == sell.ProductCode)
                .Quantity.Should().Be(QuantityAfterDelete);
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

        private void CreateSellInDatabase(int productCode, int quantity, DateTime dateTime)
        {
            sell = new SellBuilder()
                            .WithProductCode(1)
                            .WithQuantity(1)
                            .Build();
            _dataContext.Manipulate(_ => _.Sells.Add(sell));
        }
    }
}
