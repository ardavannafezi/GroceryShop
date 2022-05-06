using GroceryShop.Entities;
using GroceryShop.Services.Sells.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
