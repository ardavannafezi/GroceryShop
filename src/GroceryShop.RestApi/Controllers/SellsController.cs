using GroceryShop.Services.Imports;
using GroceryShop.Services.Sells.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GroceryShop.RestApi.Controllers
{


    [Route("api/sells")]
    [ApiController]
    public class SellsController : ControllerBase
    {
        private readonly SellAppServices _sut;

        public SellsController(SellAppServices service)
        {
            _sut = service;
        }

        [HttpPost]
        public void Add(AddSellDto dto)
        {
            _sut.Add(dto);
        }

        [HttpGet]
        public IList<GetSellsDto> GetAll()
        {
            return _sut.GetAll();
        }


        [HttpDelete("{id}")]
        public void Delete(int code)
        {
            _sut.Delete(code);
        }


        [HttpPut("{id}")]
        public void Update(UpdateSellDto dto, int id)
        {
            _sut.Update(dto, id);
        }
    }

}
