using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Category FindById(int id)
        {
            return _dataContext.Categories.FirstOrDefault(_ => _.Id == id);
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

        public bool IsCategoryExistById(string name)
        {
            return _dataContext.Categories.Any(c => c.Name == name);
        }

        public void Update(Category category)
        {
            _dataContext.Categories.Update(category);
        }
    }
    }
