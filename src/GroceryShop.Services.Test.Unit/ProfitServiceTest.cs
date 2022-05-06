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
using GroceryShop.TestTools.categories;
using GroceryShop.TestTools.Products;
using GroceryShop.TestTools.Sells;
using Xunit;

namespace GroceryShop.Services.Test.Unit
{
    public class ProfitServiceTest
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
        private Category category;
        private Product _product;
        private Import import;
        private Sell sell;

        double profit;

        public ProfitServiceTest()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _importRepository = new EFImportRepository(_dataContext);
            _sellRepository = new EFSellRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);

            _sut = new ProfitAppServices(_productRepository, _unitOfWork, _categoryRepository, _sellRepository, _importRepository);
        }


        [Fact]
        public void GeneralProfit_calculate_all_shops_profit_properly()
        {


            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            int categoryId = _categoryRepository.FindByName(category.Name).Id;
            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(categoryId)
               .WithProductCode(1)
               .WithBuyPrice(5000)
               .WithQuantity(0)
               .WithMaxInStock(10)
               .WithSellPrice(10000)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));

            import = new ImportBuilder()
            .WithProductCode(1)
            .WithQuantity(5)
            .Build();
            _dataContext.Manipulate(_ => _.Imports.Add(import));

            sell = new SellBuilder()
            .WithProductCode(1)
            .WithQuantity(5)
            .Build();
            _dataContext.Manipulate(_ => _.Sells.Add(sell));

            _sut.GetGeneralProfit().Should().Be(25000);
        }


        [Fact]
        public void CategoryProfit_calculate_all_Category_profit_properly()
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

            import = new ImportBuilder()
           .WithProductCode(1)
           .WithQuantity(5)
           .Build();
            _dataContext.Manipulate(_ => _.Imports.Add(import));

            sell = new SellBuilder()
             .WithProductCode(1)
             .WithQuantity(5)
             .Build();
            _dataContext.Manipulate(_ => _.Sells.Add(sell));

            _sut.GetCategoryProfit(category.Id).Should().Be(25000);
        }


        [Fact]
        public void ProductProfit_calculate_all_Prodcts_profit_properly()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            int categoryId = _categoryRepository.FindByName(category.Name).Id;
            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(categoryId)
               .WithProductCode(1)
               .WithBuyPrice(5000)
               .WithQuantity(0)
               .WithMaxInStock(10)
               .WithSellPrice(10000)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));


            import = new ImportBuilder()
             .WithProductCode(1)
             .WithQuantity(5)
             .Build();
            _dataContext.Manipulate(_ => _.Imports.Add(import));

            sell = new SellBuilder()
           .WithProductCode(1)
           .WithQuantity(5)
           .Build();
            _dataContext.Manipulate(_ => _.Sells.Add(sell));

            _sut.GetProductProfit(1).Should().Be(25000);

        }
    }
}
