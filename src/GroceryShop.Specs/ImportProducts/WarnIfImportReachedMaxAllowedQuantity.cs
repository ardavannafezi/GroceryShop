using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Imports;
using GroceryShop.Services.Imports;
using GroceryShop.Services.Imports.Contract;
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
    [Scenario(" تعریف ورودی کالا بیش از حداکثر")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = "ورودی کالا را مدیریت",
      InOrderTo = "ورودی تعریف کنم"
  )]
    public class WarnIfImportReachedMaxAllowedQuantity : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly ImportServices _sut;
        private readonly ImportRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Action expected;
        Category category;
        Product product;
        AddImportDto dto;

        public WarnIfImportReachedMaxAllowedQuantity(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFImportRepository(_dataContext);

            _sut = new ImportAppServices(_repository, _unitOfWork);
        }


        [Given(" کالایی با کد '01' و حداکثر موجودی 10 در فهرست کالا ها تعریف شده است ")]
        public void Given()
        {
            CreateCategoryInDatabase("labaniyat");
            CreateProductInDatabase("maste shirazi", 1, category.Id, 0,12);
        }

        [When("ورودی کالای کد '01' را به تعداد '13' و قیمت 100 وارد می کنم")]
        public void When()
        {
            CreateImportDto(product.ProductCode, 13);
            expected = () => _sut.Add(dto);
        }

        [Then(" خطای ' ورودی بیشتر از حد مجاز' رخ می دهد")]
        public void Then()
        {
            expected.Should().ThrowExactly<ReachedMaximumAllowedInStockExeption>();
        }
     
        [And("موجودی کالا بدون تغییر می باشد")]
        public void ThenAnd()
        {
            _dataContext.Products.FirstOrDefault(_ => _.ProductCode == product.ProductCode)
                .Quantity.Should().Be(product.Quantity);
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
            , _ => When()
            , _ => Then()
            , _ => ThenAnd()
         
            ) ;
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
           int maxInStock
           )
        {
            product = new ProductFactory()
                .WithName(name)
                .WithCategoryId(categoryId)
                .WithProductCode(productCode)
                .WithQuantity(quantity)
                .WithMaxInStock(maxInStock)
                .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));
        }
        private void CreateImportDto(int productCode, int quantity)
        {
            dto = new ImportDtoBuilder()
              .WithProductCode(productCode)
              .WithQuantity(quantity)
              .Build();
        }
    }
}
