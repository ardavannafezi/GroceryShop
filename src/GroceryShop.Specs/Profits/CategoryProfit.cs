using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Categories;
using GroceryShop.Persistence.EF.Imports;
using GroceryShop.Persistence.EF.Products;
using GroceryShop.Persistence.EF.Sells;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.Services.Profit;
using GroceryShop.Services.Profit.Contracts;
using GroceryShop.Services.Sells.Contracts;
using GroceryShop.Specs.Infrastructure;
using GroceryShop.TestTools.categories;
using GroceryShop.TestTools.Products;
using GroceryShop.TestTools.Sells;
using System;
using Xunit;
using static GroceryShop.Specs.BDDHelper;

namespace GroceryShop.Specs.Profit
{
    [Scenario("سود دسته")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = "سود را مدیریت کنم ",
      InOrderTo = "سود دسته را مشاهده کنم"
  )]
    public class CategoryProfit : EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices _productSut;
        private readonly ImportServices _importsut;
        private readonly SellServices _sellSut;
        private readonly ProfitServices _sut;

        private readonly ProductRepository _productRepository;
        private readonly ImportRepository _importRepository;
        private readonly SellRepository _sellRepository;

        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private Product _product;
        private Import import;
        private Sell sell;
        double profit;
        Action expected;
        Category category;

        public CategoryProfit(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _importRepository = new EFImportRepository(_dataContext);
            _sellRepository = new EFSellRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);

            _sut = new ProfitAppServices(_productRepository, _unitOfWork, _categoryRepository,_sellRepository,_importRepository );
        }


        [Given("قیمت خرید کالا هایی در دسته 5000 و فروش 10000")]
        public void Given()
        {
            category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(1)
               .WithBuyPrice(5000)
               .WithQuantity(0)
               .WithMaxInStock(10)
               .WithSellPrice(10000)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));

        }

        [And("تعداد 5 عدد وارد شده")]
        public void GivenAnd()
        {
            import = new ImportBuilder()
              .WithProductCode(1)
              .WithQuantity(5)
              .Build();
            _dataContext.Manipulate(_ => _.Imports.Add(import));
        }

        [And("تعداد 5 عدد فروش شده")]
        public void And()
        {
            sell = new SellBuilder()
              .WithProductCode(1)
              .WithQuantity(5)
              .Build();
            _dataContext.Manipulate(_ => _.Sells.Add(sell));
        }

        [When("سود را حساب می کنم")]
        public void When()
        {
            profit = _sut.GetCategoryProfit(category.Id);
        }

        [Then("سود 25000 می باشد")]
        public void Then()
        {

            profit.Should().Be(25000);

        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
            , _ => GivenAnd()
            , _ => And()
            , _ => When()
            , _ => Then());
            
        }

    }
}
