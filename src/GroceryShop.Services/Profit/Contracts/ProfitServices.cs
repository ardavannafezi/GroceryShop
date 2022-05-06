using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Profit.Contracts
{
    public interface ProfitServices
    {
        double GetProductProfit(int productcode);
        double GetCategoryProfit(int id);
        double GetGeneralProfit();
    }
}
