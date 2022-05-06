using GroceryShop.Entities;
using GroceryShop.Services.Imports.Contract;
using GroceryShop.Services.Sells.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Sells.Contracts
{
    public interface SellRepository
    {
        void Add(Sell sell);
        List<GetSellsDto> GetAll();
        bool isExist(int id);
        public void Delete(int id);
        Sell GetById(int id);
        void Update(Sell sell);
        List<Sell> GetByProduct(int id);
    }
}
