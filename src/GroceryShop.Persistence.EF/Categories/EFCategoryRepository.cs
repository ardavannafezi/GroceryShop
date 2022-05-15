using GroceryShop.Entities;
using GroceryShop.Services.Categories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace GroceryShop.Persistence.EF.Categories
{
    public class EFCategoryRepository : CategoryRepository
    {
        private readonly EFDataContext _dataContext;

        public EFCategoryRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Category category)
        {
            _dataContext.Categories.Add(category);
        }

        public void Delete(Category category)
        {
            _dataContext.Categories.Remove(category);
        }

        public Category FindById(int id)
        {
            return _dataContext.Categories.FirstOrDefault(x => x.Id == id);
        }

        public Category FindByName(string name)
        {
            return _dataContext.Categories.FirstOrDefault(x => x.Name == name);
        }

        public IList<GetCategoryDto> GetAll()
        {
            return _dataContext.Categories
               .Select(x => new GetCategoryDto
               {
                   Name = x.Name,
                   Id = x.Id,
               }).ToList();
        }

        public bool IsCategoryExist(int id)
        {
            if (_dataContext.Categories.Any(x => x.Id == id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsCategoryExistById(string name)
        {
            return _dataContext.Categories.Any(c => c.Name == name);
        }
        public bool IsCategoryByName(string name)
        {
            return _dataContext.Categories.Any(c => c.Name == name);
        }

        public bool isHavingProduct(int categoryId)
        {
            return _dataContext.Products.Any(_ => _.CategoryId == categoryId);
        }

        public void Update(Category category)
        {
            _dataContext.Categories.Update(category);
        }
    }
}
