using GroceryShop.Services.Categories;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Imports;
using GroceryShop.Services.Imports.Contract;
using GroceryShop.Services.Products;
using GroceryShop.Services.Products.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GroceryShop.RestApi.Controllers
{


    [Route("api/imports")]
    [ApiController]
    public class ImportsController : ControllerBase
    {
        private readonly ImportAppServices _sut;

        public ImportsController(ImportAppServices service)
        {
            _sut = service;
        }

        [HttpPost]
        public void Add(AddImportDto dto)
        {
            _sut.Add(dto);
        }

        [HttpGet]
        public IList<GetImportsDto> GetAll()
        {
            return _sut.GetAll();
        }


        [HttpDelete("{id}")]
        public void Delete(int code)
        {
            _sut.Delete(code);
        }


        [HttpPut("{id}")]
        public void Update(UpdateImportDto dto, int id)
        {
            _sut.Update(dto, id);
        }
    }

}
