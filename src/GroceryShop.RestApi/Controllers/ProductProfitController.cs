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


    [Route("api/productProfit")]
    [ApiController]
    public class ProductProfitController : ControllerBase
    {
        private readonly ProfitAppServices _sut;

        public ProductProfitController(ProfitAppServices service)
        {
            _sut = service;
        }

    
        [HttpGet("{id}")]
        public double GetAll(int id)
        {
            return _sut.GetProductProfit(id);
        }

    }

}
