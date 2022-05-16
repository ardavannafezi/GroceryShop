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
using GroceryShop.Services.Products;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.TestTools.categories;
using GroceryShop.TestTools.Products;

using System;
using System.Linq;
using Xunit;

namespace GroceryShop.Services.Test.Unit
{
    public class ImportProductServiceTest
    {

        private readonly EFDataContext _dataContext;
        private readonly ImportServices _sut;
        private readonly ImportRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public ImportProductServiceTest()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _repository = new EFImportRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sut = new ImportAppServices(_repository, _unitOfWork);
        }

        [Fact]
        public void Import_imports_new_product_properly ()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(1)
               .WithQuantity(0)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));


            var dto = new ImportDtoBuilder()
              .WithProductCode(1)
              .WithQuantity(5)
              .Build();

            _sut.Add(dto);

            _dataContext.Imports.Count(_ => _.ProductCode == 1)
                .Should().Be(1);

        }

        [Fact]
        public void Import_increase_quantity_to_existed_numbers()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(1)
               .WithQuantity(3)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));


            var dto = new ImportDtoBuilder()
              .WithProductCode(1)
              .WithQuantity(5)
              .Build();

            _sut.Add(dto);

            _dataContext.Products.FirstOrDefault(_ => _.ProductCode == dto.ProductCode)
                .Quantity.Should().Be(8);

        }

        [Fact]
        public void Import_throws_ProductNotFound_when_new_product_Imported_with_ProductId_that_doesnt_exist()
        {

            var dto = new ImportDtoBuilder()
                .WithProductCode(1)
                .WithQuantity(5)
                .Build();

            Action expected = () => _sut.Add(dto);
            expected.Should().ThrowExactly<ProductNotFoundExeption>();

        }

        [Fact]
        public void Import_Warns_if_you_add_more_than_accepted_quantity_to_stocks()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(1)
               .WithQuantity(1)
               .WithMaxInStock(12)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));


            var dto = new ImportDtoBuilder()
              .WithProductCode(1)
              .WithQuantity(13)
              .Build();

            Action expected = () => _sut.Add(dto); ;
            expected.Should().ThrowExactly<ReachedMaximumAllowedInStockExeption>();

            _dataContext.Products.FirstOrDefault(_ => _.ProductCode == product.ProductCode)
                .Quantity.Should().Be(product.Quantity);
        }



        [Fact]
        public void GetAll_gets_all_Existing_Imports()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(1)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));

            var import = new ImportBuilder()
                .WithProductCode(1)
                .WithQuantity(1)
                .Build();
            _dataContext.Manipulate(_ => _.Imports.Add(import));

            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.ProductCode == import.ProductCode
            && _.Quantity == import.Quantity );
        }


       
        [Fact]
        public void Delete_delete_import_from_list_properly()
        {

            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(1)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));

            var import = new ImportBuilder()
              .WithQuantity(category.Id)
              .WithProductCode(1)
              .Build();
            _dataContext.Manipulate(_ => _.Imports.Add(import));

            _sut.Delete(1);

            _unitOfWork.Commit();

            _dataContext.Imports.FirstOrDefault(_ => _.Id == import.Id)
              .Should().BeNull();

        }


        [Fact]
        public void Delete_throw_ImportNotFoundExeption_exeption_when_import_doesnt_exist()
        {
            Action expected = () => _sut.Delete(2);
            expected.Should().ThrowExactly<ImportNotFoundExeption>();
        }


        [Fact]
        public void Update_Update_import_properly()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithQuantity(8)
               .WithProductCode(1)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));

           Import import = new ImportBuilder()
                .WithProductCode(1)
                .WithQuantity(3)
                .Build();
            _dataContext.Manipulate(_ => _.Imports.Add(import));

            var dto = new UpdateImportDtoBuilder()
               .WithProductCode(1)
               .WithQuantity(5)
               .Build();

            _sut.Update(dto, import.Id);

            _dataContext.Imports.Count(_ => _.ProductCode == 1
           && _.Quantity == 5).Should().Be(1);

            int productCode = _repository.FindProductById(import.ProductCode).ProductCode;
            _dataContext.Products
                .FirstOrDefault(_ => _.ProductCode == productCode)
                .Quantity.Should().Be(10);
        }
    }
}
