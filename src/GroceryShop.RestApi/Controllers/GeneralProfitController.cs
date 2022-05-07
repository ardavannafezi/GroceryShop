using GroceryShop.Services.Categories;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Imports;
using GroceryShop.Services.Imports.Contract;
using GroceryShop.Services.Products;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.Services.Profit;
using GroceryShop.Services.Sells.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GroceryShop.RestApi.Controllers
{


    [Route("api/generalProfit")]
    [ApiController]
    public class GeneralProfitController : ControllerBase
    {
        private readonly ProfitAppServices _sut;

        public GeneralProfitController(ProfitAppServices service)
        {
            _sut = service;
        }

    
        [HttpGet]
        public double GetAll()
        {
            return _sut.GetGeneralProfit();
        }

    }

}
