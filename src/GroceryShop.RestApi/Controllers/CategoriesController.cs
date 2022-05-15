using GroceryShop.Services.Categories;
using GroceryShop.Services.Categories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GroceryShop.RestApi.Controllers
{
    [Route("api/Categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryAppService _sut;
        public CategoriesController(CategoryAppService service)
        {
            _sut = service;
        }

        [HttpPost]
        public void Add(AddCategoryDto dto)
        {
            _sut.Add(dto);
        }

        [HttpGet]
        public IList<GetCategoryDto> GetAll()
        {
            return _sut.GetAll();
        }

        [HttpDelete("{id}")]
        public void DeleteCategory(int id)
        {
            _sut.Delete(id);
        }

        [HttpPut("{id}")]
        public void UpdateCategory(UpdateCategoryDto dto, int id)
        {
            _sut.Update(dto, id);
        }
    }
}
