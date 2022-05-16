using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Categories;
using GroceryShop.Persistence.EF.Products;
using GroceryShop.Persistence.EF.Sells;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Imports;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.Services.Sells.Contract;
using GroceryShop.Services.Sells.Contracts;
using GroceryShop.Specs.Infrastructure;
using GroceryShop.TestTools.categories;
using GroceryShop.TestTools.Products;
using System;
using System.Linq;
using Xunit;
using static GroceryShop.Specs.BDDHelper;

namespace GroceryShop.Specs.SellProducts
{
    [Scenario("تعریف فروش کالا")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = "فروش کالا را مدیریت",
      InOrderTo = "فروش تعریف کنم"
  )]
    public class AddSellProduct : EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly SellServices _sut;
        private readonly SellRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Category category;
        Product product;
        AddSellDto dto;
        int LastQuantity;

        public AddSellProduct(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFSellRepository(_dataContext);
            _sut = new SellAppServices(_repository, _unitOfWork);
        }

        [Given("کالایی با کد '01' در فهرست لبنیات تعریف شده است و 6 عدد موجود است")]
        public void Given()
        {
            CreateCategoryInDatabase("labaniyat");
            CreateProductInDatabase("maste shirazi", 1, category.Id,6, 1, 10);
            LastQuantity = product.Quantity;
        }

        [When("کالای 01 را در به تعداد 2 عدد در همین لحظه می فروشیم")]
        public void When()
        {
            dto = CreateAddSellDto(1,2,DateTime.Now);
            _sut.Add(dto);
        }

        [Then("فروش کالایی با کد کالای1 و تعداد 2 در لیست فروش موجود است")]
        public void Then()
        {
            _dataContext.Sells
                .Count(_ => _.ProductCode == dto.ProductCode && _.Quantity == dto.Quantity);
        }

        [And("تعداد 4 کالا در لیست کالا ها باقی")]
        public void ThenAnd()
        {
            int NewQuantity = LastQuantity - dto.Quantity;

            Product expected =  _dataContext.Products
                .FirstOrDefault(_ => _.ProductCode == dto.ProductCode);
             expected.Quantity.Should().Be(NewQuantity);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
            , _ => When()
            , _ => Then()
            , _ => ThenAnd());
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
           int quantity,
           int min,
           int max
           )
        {
            product = new ProductFactory()
                .WithName(name)
                .WithCategoryId(categoryId)
                .WithQuantity(quantity)
                .WithProductCode(productCode)
                .WithMaxInStock(max)
                .WithMinInStock(min)
                .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));
        }
        private static AddSellDto CreateAddSellDto
            ( int productCode, int quantity , DateTime dateTime)
        {
            return new SellDtoBuilder()
              .WithProductCode(productCode)
              .WithQuantity(quantity)
              .WithDateTime(dateTime)
              .Build();
        }
    }
}