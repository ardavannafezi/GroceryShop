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
using Xunit;
using static GroceryShop.Specs.BDDHelper;

namespace GroceryShop.Specs.SellProducts
{
    [Scenario("مشاهده فروش کالا")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = "فروش کالا را مدیریت",
      InOrderTo = "فروش مشاهده کنم"
  )]
    public class GetSells : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly SellServices _sut;
        private readonly SellRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Category category;
        Product product;
        Sell sell;

        public GetSells(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFSellRepository(_dataContext);

            _sut = new SellAppServices(_repository, _unitOfWork);
        }

        [Given("کالایی با کد '01' و تعداد 3 فروش رفته شده")]
        public void Given()
        {
            CreateCategoryInDatabase("labaniyat");
            CreateProductInDatabase("maste shirazi", 1, category.Id, 6);
            CreateSellInDatabase(1, 3, DateTime.Now);
        }

        [When("میخواهیم لیست تمامی ورودی ها را دریافت کنیم")]
        public void When()
        {
            _sut.GetAll();
        }

        [Then(" فروش کالایی با '1' و تعداد 3 و به ما داده می شود")]
        public void Then()
        {
            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.ProductCode == sell.ProductCode
            && _.Quantity == sell.Quantity );
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

        private void CreateSellInDatabase(int productCode, int quantity, DateTime dateTime)
        {
            sell = new SellBuilder()
                            .WithProductCode(productCode)
                            .WithQuantity(quantity)
                            .WithDateTime(dateTime)
                            .Build();
            _dataContext.Manipulate(_ => _.Sells.Add(sell));
        }
    }
}
