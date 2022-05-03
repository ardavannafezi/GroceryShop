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
using Microsoft.EntityFrameworkCore;

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
        private readonly CategoryRepository _categoryRepository;

        public CategoryServiceTests()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _repository = new EFCategoryRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork, _categoryRepository);
        }

        [Fact]
        public void Add_adds_new_category_properly()
        {
            AddCategoryDto dto = CategoryFactory.AddCategoryDto("dummy");
            _sut.Add(dto);

            var expected = _dataContext.Categories
                .FirstOrDefault();
            expected.Name.Should().Be(dto.Name);
        }

        [Fact]
        public void Add_throws_DuplicatedCategoryNameExeption_when_new_category_added_with_name_thatis_Already_exist()
        {
            var category = new Category
            {
                Name = "dummy",
            }; _dataContext.Manipulate(_ => _.Categories.Add(category));

            var dto = CategoryFactory.AddCategoryDto("dummy");

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<DuplicatedCategoryNameExeption>();
        }

        [Fact]
        public void GetAll_gets_all_Existing_Categories()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var expected = _sut.GetAll();


           expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Name == category.Name);

        }

        [Fact]
        public void Update_Throws_DuplicatedCategoryExeption_if_new_category_name_already_exist()
        {
            var category = CategoryFactory.CreateCategory("labaniyat");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var categoryToUpdate = CategoryFactory.CreateCategory("perotoeny");
            _dataContext.Manipulate(_ => _.Categories.Add(categoryToUpdate));

            var categoryDto = CategoryFactory.UpdateCategoryDto("labaniyat");

            Action expected = () => _sut.Update(categoryDto, "perotoeny");
            expected.Should().ThrowExactly <TheCategoryNameAlreadyExist> ();

            var expectedToBe = _dataContext.Categories
                .FirstOrDefault();
            expectedToBe.Name.Should().Be(category.Name);
        }

    }
}
