using GroceryShop.Entities;
using GroceryShop.Services.Imports.Contract;
using GroceryShop.Services.Products.Contracts;
using System.Collections.Generic;
using System.Linq;

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

        public void Delete(Import import)
        {
            _dataContext.Imports.Remove(import);
        }

        public Product FindProductById(int productCode)
        {
            return _dataContext.Products
                .FirstOrDefault(Products => Products.ProductCode == productCode);
        }

        public List<GetImportsDto> GetAll()
        {
            return _dataContext.Imports
                     .Select(x => new GetImportsDto
                     {
                         ProductCode = x.ProductCode,
                         Id = x.Id,
                         Quantity = x.Quantity,
                     }).ToList();
        }

        public Import GetById(int id)
        {
            return _dataContext.Imports.FirstOrDefault(_ => _.Id == id);
        }

        public List<Import> GetByProduct(int Productid)
        {
            return _dataContext.Imports
                .Where(_ => _.ProductCode == Productid)
                      .Select(x => new Import
                      {
                          ProductCode = x.ProductCode,
                          Id = x.Id,
                          Quantity = x.Quantity,
                      }).ToList();
        }

        public bool isExist(int id)
        {
            if (_dataContext.Imports.Any(_ => _.Id == id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isProductCodeExist(int productCode)
        {
            if (_dataContext.Products
                           .Any(_ => _.ProductCode == productCode))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Update(Import import)
        {
            _dataContext.Update(import);
        }

        public int GetMaxInStock(int code)
        {
            return _dataContext.Products
                .FirstOrDefault(Products => Products.ProductCode == code)
                .MaxInStock;
        }

        public void UpdateProduct(Product product)
        {
            _dataContext.Products.Update(product);
        }
    }
}
