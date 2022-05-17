using System.Linq;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Specs.Infrastructure;
using FluentAssertions;
using Xunit;
using static GroceryShop.Specs.BDDHelper;
using BookStore.Persistence.EF;
using GroceryShop.Infrastructure.Application;
using GroceryShop.TestTools.categories;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.Persistence.EF.Products;
using GroceryShop.Services.Products;
using GroceryShop.TestTools.Products;

namespace GroceryShop.Specs.Products
{
    [Scenario("حذف کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "    کالا را مدیریت کنم",
        InOrderTo = "آنها را حذف کنم"
    )]
    public class DeleteProduct: EFDataContextDatabaseFixture
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices  _sut;
        private readonly ProductRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Category category;
        Product product;

        public DeleteProduct(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sut = new ProductAppServices(_repository, _unitOfWork);
        }

        [Given("کالایی با عنوان ماست شیرازی و کد 1 تعریف شده")]
        public void Given()
        {
            CreateCategoryInDatabase("labaniyat");
            CreateProductInDatabase("maste shirazi", 1, category.Id);

        }

        [When("کالای با کد '1' را حذف می کنیم")]
        public void When()
        {
            _sut.Delete(1);
        }

        [Then("هیچ کالایی در فهرست کالا ها با کد '1' وجود ندارد")]
        public void Then()
        {
            var expected = _dataContext.Products
                .Any(_ => _.ProductCode == product.ProductCode);
            expected.Should().BeFalse();    
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
            , _ => When()
            , _ => Then()
           );;
        }

        private void CreateCategoryInDatabase(string name)
        {
            category = CategoryFactory.CreateCategory(name);
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }

        private void CreateProductInDatabase
           (string name,
           int productCode,
           int categoryId
           )
        {
            product = new ProductFactory()
                .WithName(name)
                .WithCategoryId(categoryId)
                .WithProductCode(productCode)
                .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));
        }
    }
}
