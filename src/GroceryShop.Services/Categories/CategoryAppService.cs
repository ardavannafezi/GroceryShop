using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Services.Books.Contracts;
using GroceryShop.Services.Categories.Contracts;
using System.Collections.Generic;

namespace GroceryShop.Services.Categories
{
    public class CategoryAppService : CategoryServices
    {
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public CategoryAppService(
            CategoryRepository repositoy,
            UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repositoy;
        }

        public void Add(AddCategoryDto dto)
        {
            var isCategoryExist = _repository
                .IsCategoryExistById(dto.Name);

            if (isCategoryExist)
            {
                throw new DuplicatedCategoryNameExeption();
            }

            var category = new Category
            {
                Name = dto.Name,
            };
            _repository.Add(category);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            bool isCategoryAlreadyExist = _repository.IsCategoryExist(id);
            if (isCategoryAlreadyExist == false)
            {
                throw new CategoryNotFoundExeption();
            }

            Category category = _repository.FindById(id);
            
            bool isProductExistInCategory = _repository.isHavingProduct(category.Id);
            if (isProductExistInCategory)
            {
                throw new CategoryHasExistingProduct();
            }

            _repository.Delete(category);
            _unitOfWork.Commit();
        }

        public IList<GetCategoryDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(UpdateCategoryDto dto, int id)
        {

            bool isCategoryAlreadyExist = _repository.IsCategoryByName(dto.Name);
            if (isCategoryAlreadyExist)
            {
                throw new TheCategoryNameAlreadyExist();
            }

            Category category = _repository.FindById(id);

            category.Name = dto.Name;

            _repository.Update(category);
            _unitOfWork.Commit();
        }
    }
}
