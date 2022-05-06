using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.Services.Profit.Contracts;
using GroceryShop.Services.Sells.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Profit
{
    public class ProfitAppServices : ProfitServices
    {

        private readonly ProductRepository _productrepository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private readonly ImportRepository _importRepository;
        private readonly SellRepository _sellRepository;
        public double profit;


        public ProfitAppServices(
            ProductRepository productRepositoy,
            UnitOfWork unitOfWork,
            CategoryRepository cateogryRepository,
            SellRepository sellRepository,
            ImportRepository importRepository)
        {
            _unitOfWork = unitOfWork;
            _productrepository = productRepositoy;
            _categoryRepository = cateogryRepository;
            _sellRepository = sellRepository;
            _importRepository = importRepository;
        }

        public double GetCategoryProfit(int categoryId)
        {
            var products = _productrepository.GetByCategoryId(categoryId);
            foreach (var product in products)
            {
                double sellPrice = product.SellPrice;
                double buyPrice = product.BuyPrice;

                var imports = _importRepository.GetByProduct(product.ProductCode);
                foreach (var import in imports)
                {
                    int importQuantity = import.Quantity;

                    var sells = _sellRepository.GetByProduct(product.ProductCode);
                    foreach (var sell in sells)
                    {
                        int sellQuantity = sell.Quantity;

                        double sold = sellQuantity * sellPrice;
                        double bought = importQuantity * buyPrice;
                        profit = sold - bought;
                    }
                }
            }
            return profit;
        }


        public double GetGeneralProfit()
        {
            var products = _productrepository.GetAll();
            foreach (var product in products)
            {
                double sellPrice = product.SellPrice;
                double buyPrice = product.BuyPrice;

                var imports = _importRepository.GetByProduct(product.ProductCode);
                foreach (var import in imports)
                {
                    int importQuantity = import.Quantity;

                    var sells = _sellRepository.GetByProduct(product.ProductCode);
                    foreach (var sell in sells)
                    {
                        int sellQuantity = sell.Quantity;

                        double sold = sellQuantity * sellPrice;
                        double bought = importQuantity * buyPrice;
                        profit = sold - bought;
                    }
                }
            }
            return profit;

        }


        public double GetProductProfit(int productcode)
        {

            var product = _productrepository.GetAll()
                .FirstOrDefault(_ => _.ProductCode == productcode);
            double sellPrice = product.SellPrice;
            double buyPrice = product.BuyPrice;
            
            var imports = _importRepository.GetByProduct(product.ProductCode);
            foreach(var import in imports)
            {
                int importQuantity = import.Quantity;
                 
                var sells = _sellRepository.GetByProduct(product.ProductCode);
                foreach (var sell in sells)
                {
                    int sellQuantity = sell.Quantity;

                    double sold = sellQuantity * sellPrice;
                    double bought = importQuantity * buyPrice;
                    profit = sold - bought;
                }
            }

                return profit;
        }
   
    }
}
