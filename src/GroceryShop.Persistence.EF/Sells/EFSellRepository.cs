using GroceryShop.Entities;
using GroceryShop.Services.Sells.Contract;
using GroceryShop.Services.Sells.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace GroceryShop.Persistence.EF.Sells
{
    public class EFSellRepository : SellRepository
    {

        private readonly EFDataContext _dataContext;

        public EFSellRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Sell sell)
        {
          _dataContext.Sells.Add(sell);
        }

        public List<GetSellsDto> GetAll()
        {
            return _dataContext.Sells
                        .Select(x => new GetSellsDto
                        {
                            ProductCode = x.ProductCode,
                            Id = x.Id,
                            Quantity = x.Quantity,
             }).ToList();
        }

         public void Delete(int id)
        {
            _dataContext.Sells.Remove(GetById(id));
        }


        public Sell GetById(int id)
        {
            return _dataContext.Sells.FirstOrDefault(_ => _.Id == id);
        }

        public bool isExist(int id)
        {
            if (_dataContext.Sells.Any(_ => _.Id == id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Update(Sell sell)
        {
            _dataContext.Update(sell);
        }

        public List<Sell> GetByProduct(int id)
        {
            return _dataContext.Sells
                .Where(x => x.ProductCode == id)
                       .Select(x => new Sell
                       {
                           ProductCode = x.ProductCode,
                           Id = x.Id,
                           Quantity = x.Quantity,
                       }).ToList();
        }
    }
}
