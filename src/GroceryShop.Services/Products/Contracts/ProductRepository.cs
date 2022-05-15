using GroceryShop.Entities;
using System.Collections.Generic;

namespace GroceryShop.Services.Products.Contracts
{
    public interface ProductRepository
    {
        void Add(Product product);
        bool isProductNameExistInCategory(string name, int categoryId);
        bool isProductCodeExist(int code);
        IList<GetProductDto> GetAll();
        Product FindById(int code);
        Product FindByName(string name);
        void Update(Product product);
        bool isNameAlreadyExist(string name);
        bool isCodeAlreadyExist(int code);
        string GetNameByCode(int code);
        void Delete(Product product);
        int GetQuantity(int code);
        int GetMinInStock(int code);
        int GetMaxInStock(int code);
        List<Product> GetByCategoryId(int categoryId);
    }
}
