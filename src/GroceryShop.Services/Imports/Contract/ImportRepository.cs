using GroceryShop.Entities;
using GroceryShop.Services.Imports.Contract;
using System.Collections.Generic;

namespace GroceryShop.Services.Products.Contracts
{
    public interface ImportRepository
    {
        void Add(Import import);
        List<GetImportsDto> GetAll();
        Import GetById(int id);
        bool isExist(int id);
        void Delete(Import import);
        void Update(Import import);
        List<Import> GetByProduct(int Productid);
        bool isProductCodeExist(int productCode);
        Product FindProductById(int productCode);
        int GetMaxInStock(int productCode);
        void UpdateProduct(Product product);
    }
}
