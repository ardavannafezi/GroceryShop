using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Sells;
using GroceryShop.Services.Imports;
using GroceryShop.Services.Sells.Contract;
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
    [Scenario("ویرایش فروش  کالا")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = "فروش کالا را مدیریت",
      InOrderTo = "فروش را ویرایش کنم"
  )]
    public class UpdateSell : EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly SellServices _sut;
        private readonly SellRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Category category;
        Product product;
        Sell sell;
        int FirstQuantity;
        int SellQuantity;
        int UpdateQuantity;

        public UpdateSell(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFSellRepository(_dataContext);

            _sut = new SellAppServices(_repository, _unitOfWork);
        }

        [Given("ورودی کالا با کد '01' وفروش 3 از 6 عدد کالا در فهرست ورودی کالا ها موجود است")]
        public void Given()
        {
            CreateCategoryInDatabase("labaniyat");
            CreateProductInDatabase("maste shirazi", 1, category.Id, 6);
            FirstQuantity = product.Quantity;
            CreateSellInDatabase(1, 3, DateTime.Now);
            SellQuantity = sell.Quantity;
        }


        [When("ورودی کالا با کد 1 را به تعداد 5  ویرایش می کنیم")]
        public void When()
        {
            UpdateSellDto dto = CreateUpdateSellDto(1,5,DateTime.Now);
            UpdateQuantity = dto.Quantity;
            _sut.Update(dto, sell.Id);

        }


        [Then(" ورودی با کد کالای '01' و تعداد 1' و به ما داده می شود")]
        public void Then()
        {
           _dataContext.Sells.Any(_ => _.ProductCode == sell.ProductCode
            && _.Quantity == sell.Quantity).Should().BeTrue();
        }

        [And(" تعداد 2عدد از کالا موجود می باشد")]
        public void ThenAnd()
        {
            int NewQuantity = FirstQuantity + SellQuantity - UpdateQuantity; 

            _dataContext.Products
                .FirstOrDefault(_ => _.ProductCode == sell.ProductCode)
                .Quantity.Should().Be(NewQuantity);
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

        private static UpdateSellDto CreateUpdateSellDto(int productCode, int quntity , DateTime dateTime)
        {
            return new UpdatSellDtoBuilder()
                            .WithProductCode(productCode)
                            .WithQuantity(quntity)
                            .WithDateTime(dateTime)
                            .Build();
        }
    }
}
