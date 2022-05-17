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
using System.Linq;
using Xunit;
using static GroceryShop.Specs.BDDHelper;

namespace GroceryShop.Specs.BuyProducts
{
    [Scenario("ویرایش ورودی کالا")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = "ورودی کالا را مدیریت",
      InOrderTo = "ورودی ویرایش کنم"
  )]
    public class UpdateImport : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly ImportServices _sut;
        private readonly ImportRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        Category category;
        Product product;
        Import import;
        UpdateImportDto dto;
        int NewQuantity;

        public UpdateImport(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFImportRepository(_dataContext);

            _sut = new ImportAppServices(_repository, _unitOfWork);
        }


        [Given("کالایی با کد '01' و تعداد' 3'  وارد شده")]
        public void Given()
        {
            CreateCategoryInDatabase("labaniyat");
            CreateProductInDatabase("maste shirazi", 1, category.Id, 0);
            NewQuantity = product.Quantity;
            CreateImportInDatabase(1, 3);
            NewQuantity = NewQuantity -  import.Quantity;

        }

        [When("ورودی کالا با کد 1 را به تعداد 5  ویرایش می کنم")]
        public void When()
        {
            dto = CreateUpdateImportDto(1,5);
            NewQuantity = NewQuantity + dto.Quantity;

            _sut.Update(dto, import.Id);
        }


        [Then(" ورودی با کد کالای '01' و تعداد 1' و موجود میباشد")]
        public void Then()
        {
            _dataContext.Imports.Count(_ => _.ProductCode == dto.ProductCode
            && _.Quantity == dto.Quantity ).Should().Be(1);
        }

        [And(" تعداد 1عدد از کالا موجود می باشد")]
        public void ThenAnd()
        {
            _dataContext.Products
                .FirstOrDefault(_ => _.ProductCode == dto.ProductCode)
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

        private void CreateImportInDatabase(int productCode, int quantity)
        {
            import = new ImportBuilder()
                .WithProductCode(productCode)
                .WithQuantity(quantity)
                .Build();
            _dataContext.Manipulate(_ => _.Imports.Add(import));
        }

        private static UpdateImportDto CreateUpdateImportDto(int poductCode, int quantity)
        {
            return new UpdateImportDtoBuilder()
                .WithProductCode(poductCode)
                .WithQuantity(quantity)
                .Build();
        }
    }
}
