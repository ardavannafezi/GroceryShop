using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Categories;
using GroceryShop.Persistence.EF.Imports;
using GroceryShop.Persistence.EF.Products;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Imports;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.Specs.Infrastructure;
using GroceryShop.TestTools.categories;
using GroceryShop.TestTools.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class WarnIfImportReachedMaxAllowedQuantity : EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices _productSut;
        private readonly ImportServices _sut;

        private readonly ProductRepository _productRepository;
        private readonly ImportRepository _repository;

        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private Product _product;
        Action expected;

        public WarnIfImportReachedMaxAllowedQuantity(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFImportRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);

            _sut = new ImportAppServices(_repository, _unitOfWork, _categoryRepository , _productRepository);
        }


        [Given(" کالایی با کد '01' و حداکثر موجودی 10 در فهرست کالا ها تعریف شده است ")]
        public void Given()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            int categoryId = _categoryRepository.FindByName(category.Name).Id;
            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(categoryId)
               .WithProductCode(1)
               .WithQuantity(1)
               .WithMaxInStock(12)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));
        }

        [When("ورودی کالای کد '01' را به تعداد '13' و قیمت 100 وارد می کنیم")]
        public void When()
        {
            var dto = new ImportDtoBuilder()
              .WithProductCode(1)
              .WithQuantity(13)
              .Build();

            expected = () => _sut.Add(dto);;

        }

        [Then(" خطای ' ورودی بیشتر از حد مجاز' رخ می دهد")]
        public void Then()
        {
            expected.Should().ThrowExactly<ReachedMaximumAllowedInStockExeption>();
        }
     
        [And("موجودی کالا بدون تغییر می باشد")]
        public void ThenAnd()
        {
            _dataContext.Products.FirstOrDefault(_ => _.ProductCode == 1).Quantity.Should().Be(1);
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
    }
}
