using GroceryShop.Entities;
using GroceryShop.Services.Products.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .Any(Products => Products.ProductCode == code))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool isProductNameExist(string name)
        {
          if (_dataContext.Products
                .Any(Products => Products.Name == name))
            {
                return true;
            }
            else
            {
                return false;
            }
            
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
                   BuyPrice = x.BuyPrice,
                   SellPrice = x.SellPrice,
                   Quantity = x.Quantity,
               }).ToList();
        }

        public Product FindById(int code)
        {
           return _dataContext.Products.FirstOrDefault(Products => Products.ProductCode == code);
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
    }
}
