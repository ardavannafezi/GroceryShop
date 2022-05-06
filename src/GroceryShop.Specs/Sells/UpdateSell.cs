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
      InOrderTo = "فروش ویرایش کنم"
  )]
    public class UpdateSell : EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices _productSut;
        private readonly SellServices _sut;

        private readonly ProductRepository _productRepository;
        private readonly SellRepository _repository;

        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private Product _product;
        Action expected;
        Sell sell;

        public UpdateSell(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFSellRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);

            _sut = new SellAppServices(_repository, _unitOfWork, _categoryRepository, _productRepository);
        }

        [Given("ورودی کالا با کد '01' وفروش 1 از 6 عدد کالا در فهرست ورودی کالا ها موجود است")]
        public void Given()
        {

            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            int categoryId = _categoryRepository.FindByName(category.Name).Id;
            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(categoryId)
               .WithProductCode(1)
               .WithQuantity(6)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));

            sell = new SellBuilder()
                .WithProductCode(1)
                .WithQuantity(1)
                .Build();
            _dataContext.Manipulate(_ => _.Sells.Add(sell));

        }


        [When("ورودی کالا با کد 1 را به تعداد 5  ویرایش می کنیم")]
        public void When()
        {

            var dto = new UpdatSellDtoBuilder()
                .WithProductCode(1)
                .WithQuantity(5)
                .Build();

            _sut.Update(dto , sell.Id);

        }

        [Then(" ورودی با کد کالای '01' و تعداد 1' و به ما داده می شود")]
        public void Then()
        {
           _dataContext.Sells.Any(_ => _.ProductCode == 1
            && _.Quantity == 5 ).Should().BeTrue();
        }

        [And(" تعداد 2عدد از کالا موجود می باشد")]
        public void ThenAnd()
        {
            int productCode = _productRepository.FindById(sell.ProductCode).ProductCode;
            _dataContext.Products
                .FirstOrDefault(_ => _.ProductCode == productCode)
                .Quantity.Should().Be(2);
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
