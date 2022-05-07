using GroceryShop.Services.Categories;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Products;
using GroceryShop.Services.Products.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GroceryShop.RestApi.Controllers
{


    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductAppServices _sut;

        public ProductsController(ProductAppServices service)
        {
            _sut = service;
        }

        [HttpPost]
        public void Add(AddProductDto dto)
        {
            _sut.Add(dto);
        }

        [HttpGet]
        public IList<GetProductDto> GetAll()
        {
            return _sut.GetAll();
        }


        [HttpDelete("{id}")]
        public void Delete(int code)
        {
            _sut.Delete(code);
        }


        [HttpPut("{id}")]
        public void Update(UpdateProductDto dto, int id)
        {
            _sut.Update(dto, id);
        }
    }

}
