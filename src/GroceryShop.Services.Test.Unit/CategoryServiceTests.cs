using BookStore.Persistence.EF;
using FluentAssertions;
using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Infrastructure.Test;
using GroceryShop.Persistence.EF;
using GroceryShop.Persistence.EF.Categories;
using GroceryShop.Services.Books.Contracts;
using GroceryShop.Services.Categories;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.TestTools.categories;
using GroceryShop.TestTools.Products;
using System;
using System.Linq;
using Xunit;

namespace GroceryShop.Services.Test.Unit
{
    public class CategoryServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryServices _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category category;
        private Category categoryToUpdate;

        public CategoryServiceTests()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _repository = new EFCategoryRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_new_category_properly()
        {
            AddCategoryDto dto = CategoryFactory.AddCategoryDto("dummy");

            _sut.Add(dto);

            var expected = _dataContext.Categories.FirstOrDefault();
            expected.Name.Should().Be(dto.Name);
        }

        [Fact]
        public void Add_throws_DuplicatedCategoryNameExeption_when_new_category_added_with_name_thatis_Already_exist()
        {
            CreateCategoryInDatabase("dummy");

            var dto = CategoryFactory.AddCategoryDto("dummy");
            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<DuplicatedCategoryNameExeption>();
        }

        [Fact]
        public void GetAll_gets_all_Existing_Categories()
        {
            CreateCategoryInDatabase("labaniyat");

            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Name == category.Name
                && _.Id == category.Id);
        }

        [Fact]
        public void Update_Throws_DuplicatedCategoryExeption_if_new_category_name_already_exist()
        {
            CreateCategoryInDatabase("labaniyat");
            CreateCategoryInDatabaseToUpdate("perotoeny");
            var categoryDto = CategoryFactory.UpdateCategoryDto("labaniyat");

            Action expected = () => _sut.Update(categoryDto, categoryToUpdate.Id);

            expected.Should().ThrowExactly<TheCategoryNameAlreadyExist>();
            var expectedToBe = _dataContext.Categories
                .FirstOrDefault();
            expectedToBe.Name.Should().Be(category.Name);
        }

        [Fact]
        public void Delete_delete_category_properly()
        {
            CreateCategoryInDatabase("labaniyat");
            _sut.Delete(category.Id);
            _unitOfWork.Commit();

            _dataContext.Categories.Any(_ => _.Name == "labaniyat").Should().BeFalse();
        }

        [Fact]
        public void Delete_Category_that_not_exist_should_throw_CategoryNotFoundExeption()
        {
            category = CategoryFactory.CreateCategory("dummy");

            Action expected = () => _sut.Delete(category.Id);

            expected.Should().ThrowExactly<CategoryNotFoundExeption>();
        }

        [Fact]
        public void Delete_ThrowCategoryHAsExistedProductExeprtion_when_aProduct_already_exist_in_category()
        {
            CreateCategoryInDatabase("labaniyat");
            CreateProductInDatabse( 2, "maste shirazi", category.Id );

            Action expected = () => _sut.Delete(category.Id);

            expected.Should().ThrowExactly<CategoryHasExistingProduct>();
        }

        private void CreateProductInDatabse(int productCode, string name,int categoryId)
        {
            var product = new ProductFactory()
               .WithName(name)
               .WithCategoryId(categoryId)
               .WithProductCode(productCode)
               .Build();

            _dataContext.Manipulate(_ => _.Products.Add(product));
        }
        public void CreateCategoryInDatabase(string name)
        {
            category = CategoryFactory.CreateCategory(name);
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }
        private void CreateCategoryInDatabaseToUpdate(string name)
        {
            categoryToUpdate = CategoryFactory.CreateCategory(name);
            _dataContext.Manipulate(_ => _.Categories.Add(categoryToUpdate));
        }
    }
}
