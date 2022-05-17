using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Products;
using GroceryShop.Services.Products;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.TestTools.categories;
using GroceryShop.TestTools.Products;

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

        public ProductServiceTests()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _repository = new EFProductRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sut = new ProductAppServices(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_new_category_properly()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductDtoBuilder()
              .WithName("maste kaleh")
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

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(2)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));


            var dto = new ProductDtoBuilder()
               .WithName("maste shirazi")
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

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(2)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));


            var dto = new ProductDtoBuilder()
              .WithName("maste kaleh")
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

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
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

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(2)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));

            var dto = new UpdateProductDtoBuilder()
               .WithName("maste kaleh")
               .WithCategoryId(category.Id)
               .WithProductCode(2)
               .Build();
            _sut.Update(dto, 2);

            var expected = _dataContext.Products.Any(_ => _.ProductCode == dto.ProductCode && _.Name == dto.Name);
            expected.Should().BeTrue();

        }

        [Fact]
        public void Update_Throws_DuplicatedNameExeption_if_new_Product_name_already_exist()
        {

            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(2)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));


            var productforEdit = new ProductFactory()
               .WithName("maste kaleh")
               .WithCategoryId(category.Id)
               .WithProductCode(3)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(productforEdit));

            var dto = new UpdateProductDtoBuilder()
               .WithName("maste shirazi")
               .WithProductCode(3)
               .Build();

            Action expected = () => _sut.Update(dto, 3);
            expected.Should().ThrowExactly<ProductNameIsDuplicatedExeption>();

            _dataContext.Products.Any(_ => _.ProductCode == product.ProductCode && 
            _.Name == product.Name).Should().BeTrue();

        }


        [Fact]
        public void Update_Throws_DuplicatedCodeExeption_if_new_Product_Code_already_exist()
        {

            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(2)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));


            var productforEdit = new ProductFactory()
               .WithName("maste kaleh")
               .WithCategoryId(category.Id)
               .WithProductCode(3)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(productforEdit));

            var dto = new UpdateProductDtoBuilder()
               .WithName("maste kaleh")
               .WithProductCode(2)
               .Build();

            Action expected = () => _sut.Update(dto, 3);
            expected.Should().ThrowExactly<ProductCodeIsDuplicatedExeption>();

            _dataContext.Products.Any(_ => _.ProductCode == product.ProductCode &&
            _.Name == product.Name).Should().BeTrue();

        }

        [Fact]
        public void Delete_delete_product_properly()
        {

            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(2)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product)); 

            _sut.Delete(product.ProductCode);
            _unitOfWork.Commit();

             _dataContext.Products.Any(_ =>
                 _.ProductCode == product.ProductCode)
                     .Should().BeFalse();
            ;
        }


        [Fact]
        public void Delete_throw_ProductNotFound_exeption_when_code_doesnt_exist()
        {

            Action expected = () => _sut.Delete(2);
            expected.Should().ThrowExactly<ProductNotFoundExeption>();          
        }

    }
}
