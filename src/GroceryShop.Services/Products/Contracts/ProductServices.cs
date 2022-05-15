using GroceryShop.Services.Categories.Contracts;
using System.Collections.Generic;

namespace GroceryShop.Services.Products.Contracts
{
    public interface ProductServices
    {
        void Add(AddProductDto product);
        IList<GetProductDto> GetAll();
        void Update(UpdateProductDto dto, int id);
        void Delete(int code);
    }
}
