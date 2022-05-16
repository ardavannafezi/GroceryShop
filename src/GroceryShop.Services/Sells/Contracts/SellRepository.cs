using GroceryShop.Entities;
using GroceryShop.Services.Sells.Contract;
using System.Collections.Generic;

namespace GroceryShop.Services.Sells.Contracts
{
    public interface SellRepository
    {
        void Add(Sell sell);
        List<GetSellsDto> GetAll();
        bool isExist(int id);
        public void Delete(Sell sell);
        Sell GetById(int id);
        void Update(Sell sell);
        List<Sell> GetByProduct(int id);
        bool isProductCodeExist(int productCode);
        Product FindProductById(int productCode);
        void UpdateProduct(Product product);
        int GetProductMaxInStock(int productCode);
        int GetProductMinInStock(int productCode);

    }
}
