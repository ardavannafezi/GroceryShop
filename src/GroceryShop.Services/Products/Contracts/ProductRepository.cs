using GroceryShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Products.Contracts
{
    public interface ProductRepository
    {
        void Add(Product product);
        bool isProductNameExist(string name);
        bool isProductCodeExist(int code);
        IList<GetProductDto> GetAll();
        Product FindById(int code);
        void Update(Product product);
        bool isNameAlreadyExist(string name);
        bool isCodeAlreadyExist(int code);
        string GetNameByCode(int code);
        void Delete(Product product);
    }
}
