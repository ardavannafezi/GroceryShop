

using GroceryShop.Services.Sells.Contract;

namespace GroceryShop.Services.Sells.Contracts
{
    public interface SellServices
    {
        void Add(AddSellDto dto);
        List<GetSellsDto> GetAll();
    }
}
