using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Categories;
using GroceryShop.Persistence.EF.Products;
using GroceryShop.Services.Books.Contracts;
using GroceryShop.Services.Categories;
using GroceryShop.Services.Categories.Contracts;
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
    public class ProductServiceTests
    {

        private readonly EFDataContext _dataContext;
        private readonly ProductServices _sut;
        private readonly ProductRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;

        public ProductServiceTests()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _repository = new EFProductRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new ProductAppServices(_repository, _unitOfWork, _categoryRepository);
        }

        [Fact]
        public void Add_adds_new_category_properly()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductDtoBuilder()
              .WithName("maste kaleh")
              .WithCategoryName("labaniyat")
              .WithProductCode(2)
              .Build();

            _sut.Add(product);


            var expected = _dataContext.Products.FirstOrDefault(_ => _.Name == product.Name);
            expected.Should().NotBeNull();
        }

        [Fact]
        public void Add_throws_DuplicatedPRoductNameExeption_when_new_product_added_with_name_thatis_Already_exist()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            int categoryId = _categoryRepository.FindByName(category.Name).Id;
            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(categoryId)
               .WithProductCode(2)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));


            var dto = new ProductDtoBuilder()
               .WithName("maste shirazi")
               .WithCategoryName("labaniyat")
               .WithProductCode(3)
               .Build();

            Action expected = () => _sut.Add(dto);
            expected.Should().ThrowExactly<ProductNameIsDuplicatedExeption>();

            _dataContext.Products.Count(_ => _.Name == product.Name).Should().Be(1);

        }

        [Fact]
        public void Add_throws_DuplicatedPRoductCodeExeption_when_new_product_added_with_ProductCode_thatis_Already_exist()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            int categoryId = _categoryRepository.FindByName(category.Name).Id;
            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(categoryId)
               .WithProductCode(2)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));


            var dto = new ProductDtoBuilder()
              .WithName("maste kaleh")
              .WithCategoryName("labaniyat")
              .WithProductCode(2)
              .Build();

            Action expected = () => _sut.Add(dto);
            expected.Should().ThrowExactly<ProductCodeIsDuplicatedExeption>();

            _dataContext.Products.Count(_ => _.ProductCode == product.ProductCode).Should().Be(1);

        }

        [Fact]
        public void GetAll_gets_all_Existing_Products()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            int categoryId = _categoryRepository.FindByName(category.Name).Id;
            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(categoryId)
               .WithProductCode(2)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));

            var expected = _sut.GetAll();


            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Name == product.Name);

        }

        [Fact]
        public void Update_updates_produvt_wih_given_informations()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            int categoryId = _categoryRepository.FindByName(category.Name).Id;
            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(categoryId)
               .WithProductCode(2)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));

            var dto = new UpdateProductDtoBuilder()
               .WithName("maste kaleh")
               .WithCategoryName("labaniyat")
               .WithProductCode(2)
               .Build();
            _sut.Update(dto, 2);

            var expected = _dataContext.Products.Any(_ => _.ProductCode == dto.ProductCode && _.Name == dto.Name);
            expected.Should().BeTrue();

        }

        //[Fact]
        //public void Update_Throws_DuplicatedNameExeption_if_new_Product_name_already_exist()
        //{
        //    var category = CategoryFactory.CreateCategory("labaniyat");
        //    _dataContext.Manipulate(_ => _.Categories.Add(category));

        //    var categoryToUpdate = CategoryFactory.CreateCategory("perotoeny");
        //    _dataContext.Manipulate(_ => _.Categories.Add(categoryToUpdate));

        //    var categoryDto = CategoryFactory.UpdateCategoryDto("labaniyat");

        //    Action expected = () => _sut.Update(categoryDto, "perotoeny");
        //    expected.Should().ThrowExactly<TheCategoryNameAlreadyExist>();

        //    var expectedToBe = _dataContext.Categories
        //        .FirstOrDefault();
        //    expectedToBe.Name.Should().Be(category.Name);
        //}

        //[Fact]
        //public void Delete_delete_category_properly()
        //{
        //    var category = CategoryFactory.CreateCategory("labaniyat");
        //    _dataContext.Manipulate(_ => _.Categories.Add(category));

        //    _sut.Delete(category.Name);
        //    _unitOfWork.Commit();

        //    _dataContext.Categories.FirstOrDefault(_ => _.Name == category.Name).Should().BeNull();
        //}

        //[Fact]
        //public void Delete_Category_that_not_exist_should_throw_CategoryNotFoundExeption()
        //{
        //    string categoryName = "DoesNotExistDummy";
        //    Action expected = () => _sut.Delete(categoryName);
        //    expected.Should().ThrowExactly<CategoryNotFoundExeption>();
        //    _unitOfWork.Commit();    
        //}
    }
}
