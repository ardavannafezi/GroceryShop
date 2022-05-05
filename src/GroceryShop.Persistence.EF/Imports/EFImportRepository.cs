using GroceryShop.Entities;
using GroceryShop.Services.Imports.Contract;
using GroceryShop.Services.Products.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Persistence.EF.Imports
{
    public class EFImportRepository :ImportRepository
    {
        private readonly EFDataContext _dataContext;

        public EFImportRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Import import)
        {
            _dataContext.Add(import);
        }

        public List<GetImportsDto> GetAll()
        {
            return _dataContext.Imports
                     .Select(x => new GetImportsDto
                     {
                         ProductCode = x.ProductCode,
                         Id = x.Id,
                         Price = x.Price,
                         Quantity = x.Quantity,
                     }).ToList();
        }
    }
}
