using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Services.Books.Contracts;
using GroceryShop.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Categories
{
    public class CategoryAppService : CategoryServices
    {
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;

        public CategoryAppService(
            CategoryRepository repositoy,
            UnitOfWork unitOfWork,
            CategoryRepository cateogryRepository)
        {
            _unitOfWork = unitOfWork;
            _repository = repositoy;
            _categoryRepository = cateogryRepository;
        }

        public void Add(AddCategoryDto dto)
        {
            var isCategoryExist = _categoryRepository
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

        public IList<GetCategoryDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(UpdateCategoryDto dto, string name)
        {
            Category category = _repository.FindByName(name);

            category.Name = dto.Name;

            _repository.Update(category);
            _unitOfWork.Commit();

        }

        public void Update(UpdateCategoryDto dto, int id)
        {
            throw new NotImplementedException();
        }
    }
}
