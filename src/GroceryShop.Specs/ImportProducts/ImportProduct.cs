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
    [Scenario("تعریف ورودی کالا")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = "ورودی کالا را مدیریت",
      InOrderTo = "ورودی تعریف کنم"
  )]
    public class ImportProduct : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly ImportServices _sut;
        private readonly ImportRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Category category;
        Product product;
        Action expected;
        AddImportDto dto;

        public ImportProduct(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFImportRepository(_dataContext);

            _sut = new ImportAppServices(_repository, _unitOfWork);
        }


        [Given("کالایی با کد '01' در فهرست کالا ها تعریف شده است و هیچ کالایی موجود 	نیست")]
        public void Given()
        {
            CreateCategoryInDatabase("labaniyat");
            CreateProductInDatabase("maste shirazi", 1, category.Id, 0);
        }

        [When("ورودی کالای کد '01' را به تعداد '5'  وارد می کنم")]
        public void When()
        {
            CreateImportDto(1,5);

            _sut.Add(dto);
        }
   
        [When("ورودی کالای کد '01' را به تعداد '5'موجود می باشد")]
        public void Then()
        {
           _dataContext.Imports.Any(_ => _.ProductCode == dto.ProductCode 
                && _.Quantity == dto.Quantity).Should().BeTrue();

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
        private void CreateImportDto(int productCode, int quantity)
        {
            dto = new ImportDtoBuilder()
              .WithProductCode(productCode)
              .WithQuantity(quantity)
              .Build();
        }
    }
}
