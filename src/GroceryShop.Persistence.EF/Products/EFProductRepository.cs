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
    }
}
