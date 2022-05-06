

using GroceryShop.Services.Sells.Contract;
using System.Collections.Generic;

namespace GroceryShop.Services.Sells.Contracts
{
    public interface SellServices
    {
        void Add(AddSellDto dto);
        List<GetSellsDto> GetAll();
        void Delete(int id);
        void Update(UpdateSellDto dto, int id);
    }
}
