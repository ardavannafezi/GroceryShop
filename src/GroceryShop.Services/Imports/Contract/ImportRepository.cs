using GroceryShop.Entities;
using GroceryShop.Services.Imports.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Products.Contracts
{
    public interface ImportRepository
    {
        void Add(Import import);
        List<GetImportsDto> GetAll();
        Import GetById(int id);
        bool isExist(int id);
        void Delete(int id);
        void Update(Import import);
        List<Import> GetByProduct(int Productid);
    }
}
