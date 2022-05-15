using GroceryShop.Services.Imports.Contract;
using System.Collections.Generic;

namespace GroceryShop.Services.Products.Contracts
{
    public interface ImportServices
    {
        public void Add(AddImportDto dto);
        public List<GetImportsDto> GetAll();
        void Delete(int id);
        void Update(UpdateImportDto dto, int id);
    }
}
