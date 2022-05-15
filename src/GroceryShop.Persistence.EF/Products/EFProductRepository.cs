using GroceryShop.Entities;
using GroceryShop.Services.Products.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace GroceryShop.Persistence.EF.Products
{
    public class EFProductRepository :ProductRepository
    {
        private readonly EFDataContext _dataContext;

        public EFProductRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Product product)
        {
            _dataContext.Add(product);
        }

        public bool isProductCodeExist(int code)
        {
            if (_dataContext.Products
                .Any(_ => _.ProductCode == code ))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isProductNameExistInCategory
            (string name,int categoryId)
        {
          if (_dataContext.Products
                .Any(_ => _.Name == name && _.CategoryId == categoryId))
            {
                return true;
            }
            else
            {
                return false;
            }   
        }

        public int GetQuantity(int code)
        {
            return _dataContext.Products
                .FirstOrDefault(Products => Products.ProductCode == code)
                .Quantity;
        }

        public int GetMaxInStock(int code)
        {
            return _dataContext.Products
                .FirstOrDefault(Products => Products.ProductCode == code)
                .MaxInStock;
        }
        public int GetMinInStock(int code)
        {
            return _dataContext.Products
                .FirstOrDefault(Products => Products.ProductCode == code)
                .MinInStock;
        }

        public IList<GetProductDto> GetAll()
        {
            return _dataContext.Products
               .Select(x => new GetProductDto
               {
                   ProductCode = x.ProductCode,
                   Name = x.Name,
                   CategoryId = x.CategoryId,
                   MaxInStock = x.MaxInStock,
                   MinInStock = x.MinInStock,
                   Quantity = x.Quantity,
               }).ToList();
        }

        public Product FindById(int code)
        {
           return _dataContext.Products.FirstOrDefault(Products => Products.ProductCode == code);
        }
        public Product FindByName(string name)
        {
            return _dataContext.Products.FirstOrDefault(Products => Products.Name == name);
        }

        public void Update(Product product)
        {
            _dataContext.Update(product);
        }

        public bool isNameAlreadyExist(string name)
        {
            if (_dataContext.Products.Any(Products => Products.Name == name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isCodeAlreadyExist(int code)
        {
            if (_dataContext.Products.Any(Products => Products.ProductCode == code))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetNameByCode(int code)
        {
            return _dataContext.Products.FirstOrDefault(Products=>Products.ProductCode == code).Name;
        }

        public void Delete(Product product)
        {
            _dataContext.Remove(product);
        }

        public List<Product> GetByCategoryId(int categoryId)
        {
            return _dataContext.Products
               .Where(_ => _.CategoryId == categoryId)
               .Select(x => new Product
               {
                   ProductCode = x.ProductCode,
                   Name = x.Name,
                   CategoryId = x.CategoryId,
                   MaxInStock = x.MaxInStock,
                   MinInStock = x.MinInStock,
                   Quantity = x.Quantity,
               }).ToList();
        }
    }
}
