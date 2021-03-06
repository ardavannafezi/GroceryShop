using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Sells;
using GroceryShop.Services.Imports;
using GroceryShop.Services.Products;
using GroceryShop.Services.Sells.Contracts;
using GroceryShop.TestTools.categories;
using GroceryShop.TestTools.Products;
using GroceryShop.TestTools.Sells;
using System;
using System.Linq;
using Xunit;

namespace GroceryShop.Services.Test.Unit
{
    public class SellProductServiceTest
    {

        private readonly EFDataContext _dataContext;
        private readonly SellServices _sut;
        private readonly SellRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public SellProductServiceTest()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _repository = new EFSellRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sut = new SellAppServices(_repository, _unitOfWork);
        }

        [Fact]
        public void Sell_Sells_new_product_properly ()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(1)
               .WithQuantity(6)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));


            var dto = new SellDtoBuilder()
           .WithProductCode(1)
           .WithQuantity(2)
           .Build();

            _sut.Add(dto);

            _dataContext.Sells.Count(_ => _.ProductCode == dto.ProductCode
                && _.Quantity == dto.Quantity);
            _dataContext.Products.FirstOrDefault(_ => _.ProductCode == dto.ProductCode)
                .Quantity.Should().Be(4);

        }

        [Fact]
        public void Sell_Throw_NotenooughtInStockExeption_when_product_sells_more_than_in_stocks()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(1)
               .WithQuantity(2)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));


            var dto = new SellDtoBuilder()
           .WithProductCode(1)
           .WithQuantity(4)
           .Build();

            Action expected = () => _sut.Add(dto);
            expected.Should().ThrowExactly<NotEcoughtInStock>();

        }

        [Fact]
        public void Sell_throws_ProductNotFound_when_new_product_Sells_with_ProductId_that_doesnt_exist()
        {

            var dto = new SellDtoBuilder()
                .WithProductCode(1)
                .WithQuantity(5)
                .Build();

            Action expected = () => _sut.Add(dto);
            expected.Should().ThrowExactly<ProductNotFoundExeption>();

        }


        [Fact]
        public void GetAll_gets_all_Existing_Sells()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(1)
               .WithQuantity(6)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));

            Sell sell = new SellBuilder()
                .WithProductCode(1)
                .WithQuantity(3)
                .Build();
            _dataContext.Manipulate(_ => _.Sells.Add(sell));

            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.ProductCode == sell.ProductCode
            && _.Quantity == sell.Quantity);
        }

        [Fact]
        public void Delete_deletes_sell_properly_and_correct_in_stock_quantity()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(1)
               .WithQuantity(6)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));

            Sell sell = new SellBuilder()
                .WithProductCode(1)
                .WithQuantity(1)
                .Build();
            _dataContext.Manipulate(_ => _.Sells.Add(sell));

            _sut.Delete(sell.Id);

            _dataContext.Imports.FirstOrDefault(_ => _.Id == sell.Id)
                .Should().BeNull();

            int productCode = _repository.FindProductById(sell.ProductCode).ProductCode;
            _dataContext.Products
                .FirstOrDefault(_ => _.ProductCode == productCode)
                .Quantity.Should().Be(7);
        }


        [Fact]
        public void Delete_throw_SellNotFoundExeption_exeption_when_sell_doesnt_exist()
        {
            Action expected = () => _sut.Delete(2);
            expected.Should().ThrowExactly<SellNotFoundExeption>();
        }

        [Fact]
        public void Update_updates_Sell_properly_and_correcting_existed_products_quantity()
        {

            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var product = new ProductFactory()
               .WithName("maste shirazi")
               .WithCategoryId(category.Id)
               .WithProductCode(1)
               .WithQuantity(6)
               .Build();
            _dataContext.Manipulate(_ => _.Products.Add(product));

            Sell sell = new SellBuilder()
                .WithProductCode(1)
                .WithQuantity(1)
                .Build();
            _dataContext.Manipulate(_ => _.Sells.Add(sell));

            var dto = new UpdatSellDtoBuilder()
               .WithProductCode(1)
               .WithQuantity(5)
               .Build();

            _sut.Update(dto, sell.Id);

           _dataContext.Sells.Any(_ => _.ProductCode == 1
                && _.Quantity == 5).Should().BeTrue();

            int productCode = _repository.FindProductById(sell.ProductCode).ProductCode;
            _dataContext.Products
                .FirstOrDefault(_ => _.ProductCode == productCode)
                .Quantity.Should().Be(2);
        }

        [Fact]
        public void Update_throw_SellNotFoundExeption_exeption_when_sell_doesnt_exist()
        {
            var dto = new UpdatSellDtoBuilder()
           .WithProductCode(1)
           .WithQuantity(5)
           .Build();

            Action expected = () => _sut.Update(dto,2);
            expected.Should().ThrowExactly<SellNotFoundExeption>();
        }
    }
}
