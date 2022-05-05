using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Categories;
using GroceryShop.Persistence.EF.Imports;
using GroceryShop.Persistence.EF.Products;
using GroceryShop.Services.Books.Contracts;
using GroceryShop.Services.Categories;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Imports;
using GroceryShop.Services.Products;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.TestTools.categories;
using GroceryShop.TestTools.Products;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using Xunit;

namespace GroceryShop.Services.Test.Unit
{
    public class ImportProductServiceTest
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices _productSut;
        private readonly ImportServices _sut;
        private readonly ImportRepository _repository;
        private readonly ProductRepository _productRepository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;

        public ImportProductServiceTest()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _productRepository = new EFProductRepository(_dataContext);
            _repository = new EFImportRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new ImportAppServices(_repository, _unitOfWork, _categoryRepository, _productRepository);
        }

        [Fact]
        public void Import_imports_new_product_properly ()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            int categoryId = _categoryRepository.FindByName(category.Name).Id;
            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(categoryId)
               .WithProductCode(1)
               .WithQuantity(0)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));


            var dto = new ImportDtoBuilder()
              .WithProductCode(1)
              .WithQuantity(5)
              .WithPrice(100)
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

            int categoryId = _categoryRepository.FindByName(category.Name).Id;
            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(categoryId)
               .WithProductCode(1)
               .WithQuantity(3)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));


            var dto = new ImportDtoBuilder()
              .WithProductCode(1)
              .WithQuantity(5)
              .WithPrice(100)
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
                .WithPrice(100)
                .Build();

            Action expected = () => _sut.Add(dto);
            expected.Should().ThrowExactly<ProductNotFoundExeption>();

        }

        //[Fact]
        //public void Add_throws_DuplicatedPRoductCodeExeption_when_new_product_added_with_ProductCode_thatis_Already_exist()
        //{
        //    var category = CategoryFactory.CreateCategory("labaniyat");
        //    _dataContext.Manipulate(_ => _.Categories.Add(category));

        //    int categoryId = _categoryRepository.FindByName(category.Name).Id;
        //    var product = new ProductFactory()
        //       .WithName("maste shirazi")
        //       .WithCategoryId(categoryId)
        //       .WithProductCode(2)
        //       .Build();
        //    _dataContext.Manipulate(_ => _.Products.Add(product));


        //    var dto = new ProductDtoBuilder()
        //      .WithName("maste kaleh")
        //      .WithCategoryName("labaniyat")
        //      .WithProductCode(2)
        //      .Build();

        //    Action expected = () => _sut.Add(dto);
        //    expected.Should().ThrowExactly<ProductCodeIsDuplicatedExeption>();

        //    _dataContext.Products.Count(_ => _.ProductCode == product.ProductCode).Should().Be(1);

        //}

        //[Fact]
        //public void GetAll_gets_all_Existing_Products()
        //{
        //    var category = CategoryFactory.CreateCategory("labaniyat");
        //    _dataContext.Manipulate(_ => _.Categories.Add(category));

        //    int categoryId = _categoryRepository.FindByName(category.Name).Id;
        //    var product = new ProductFactory()
        //       .WithName("maste shirazi")
        //       .WithCategoryId(categoryId)
        //       .WithProductCode(2)
        //       .Build();
        //    _dataContext.Manipulate(_ => _.Products.Add(product));

        //    var expected = _sut.GetAll();


        //    expected.Should().HaveCount(1);
        //    expected.Should().Contain(_ => _.Name == product.Name);

        //}

        //[Fact]
        //public void Update_updates_produvt_wih_given_informations()
        //{
        //    var category = CategoryFactory.CreateCategory("labaniyat");
        //    _dataContext.Manipulate(_ => _.Categories.Add(category));

        //    int categoryId = _categoryRepository.FindByName(category.Name).Id;
        //    var product = new ProductFactory()
        //       .WithName("maste shirazi")
        //       .WithCategoryId(categoryId)
        //       .WithProductCode(2)
        //       .Build();
        //    _dataContext.Manipulate(_ => _.Products.Add(product));

        //    var dto = new UpdateProductDtoBuilder()
        //       .WithName("maste kaleh")
        //       .WithCategoryName("labaniyat")
        //       .WithProductCode(2)
        //       .Build();
        //    _sut.Update(dto, 2);

        //    var expected = _dataContext.Products.Any(_ => _.ProductCode == dto.ProductCode && _.Name == dto.Name);
        //    expected.Should().BeTrue();

        //}

        //[Fact]
        //public void Update_Throws_DuplicatedNameExeption_if_new_Product_name_already_exist()
        //{

        //    var category = CategoryFactory.CreateCategory("labaniyat");
        //    _dataContext.Manipulate(_ => _.Categories.Add(category));

        //    int categoryId = _categoryRepository.FindByName(category.Name).Id;
        //    var product = new ProductFactory()
        //       .WithName("maste shirazi")
        //       .WithCategoryId(categoryId)
        //       .WithProductCode(2)
        //       .Build();
        //    _dataContext.Manipulate(_ => _.Products.Add(product));


        //    int categoryIdforedit = _categoryRepository.FindByName("labaniyat").Id;
        //    var productforEdit = new ProductFactory()
        //       .WithName("maste kaleh")
        //       .WithCategoryId(categoryId)
        //       .WithProductCode(3)
        //       .Build();
        //    _dataContext.Manipulate(_ => _.Products.Add(productforEdit));

        //    var dto = new UpdateProductDtoBuilder()
        //       .WithName("maste shirazi")
        //       .WithCategoryName("labaniyat")
        //       .WithProductCode(3)
        //       .Build();

        //    Action expected = () => _sut.Update(dto, 3);
        //    expected.Should().ThrowExactly<ProductNameIsDuplicatedExeption>();

        //    _dataContext.Products.Any(_ => _.ProductCode == product.ProductCode && 
        //    _.Name == product.Name).Should().BeTrue();

        //}


        //[Fact]
        //public void Update_Throws_DuplicatedCodeExeption_if_new_Product_Code_already_exist()
        //{

        //    var category = CategoryFactory.CreateCategory("labaniyat");
        //    _dataContext.Manipulate(_ => _.Categories.Add(category));

        //    int categoryId = _categoryRepository.FindByName(category.Name).Id;
        //    var product = new ProductFactory()
        //       .WithName("maste shirazi")
        //       .WithCategoryId(categoryId)
        //       .WithProductCode(2)
        //       .Build();
        //    _dataContext.Manipulate(_ => _.Products.Add(product));


        //    int categoryIdforedit = _categoryRepository.FindByName("labaniyat").Id;
        //    var productforEdit = new ProductFactory()
        //       .WithName("maste kaleh")
        //       .WithCategoryId(categoryId)
        //       .WithProductCode(3)
        //       .Build();
        //    _dataContext.Manipulate(_ => _.Products.Add(productforEdit));

        //    var dto = new UpdateProductDtoBuilder()
        //       .WithName("maste kaleh")
        //       .WithCategoryName("labaniyat")
        //       .WithProductCode(2)
        //       .Build();

        //    Action expected = () => _sut.Update(dto, 3);
        //    expected.Should().ThrowExactly<ProductCodeIsDuplicatedExeption>();

        //    _dataContext.Products.Any(_ => _.ProductCode == product.ProductCode &&
        //    _.Name == product.Name).Should().BeTrue();

        //}

        //[Fact]
        //public void Delete_delete_product_properly()
        //{

        //    var category = CategoryFactory.CreateCategory("labaniyat");
        //    _dataContext.Manipulate(_ => _.Categories.Add(category));

        //    int categoryId = _categoryRepository.FindByName(category.Name).Id;
        //    var product = new ProductFactory()
        //       .WithName("maste shirazi")
        //       .WithCategoryId(categoryId)
        //       .WithProductCode(2)
        //       .Build();
        //    _dataContext.Manipulate(_ => _.Products.Add(product)); 

        //    _sut.Delete(product.ProductCode);
        //    _unitOfWork.Commit();

        //     _dataContext.Products.Any(_ =>
        //         _.ProductCode == product.ProductCode)
        //             .Should().BeFalse();
        //    ;
        //}


        //[Fact]
        //public void Delete_throw_ProductNotFound_exeption_when_code_doesnt_exist()
        //{

        //    Action expected = () => _sut.Delete(2);
        //    expected.Should().ThrowExactly<ProductNotFoundExeption>();          
        //}

    }
}