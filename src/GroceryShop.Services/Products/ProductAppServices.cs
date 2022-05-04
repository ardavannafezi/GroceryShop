using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Products.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Products
{
    public class ProductAppServices : ProductServices
    {
        private readonly ProductRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;

        public ProductAppServices(
            ProductRepository repositoy,
            UnitOfWork unitOfWork,
            CategoryRepository cateogryRepository)
        {
            _unitOfWork = unitOfWork;
            _repository = repositoy;
            _categoryRepository = cateogryRepository;
        }

        public void Add(AddProductDto dto)
        {

            bool isProductNameExist = _repository.isProductNameExist(dto.Name);
            if (isProductNameExist)
            {
                throw new ProductNameIsDuplicatedExeption();
            }

            bool isProductCodeExist = _repository.isProductCodeExist(dto.ProductCode);
            if (isProductCodeExist)
            {
                throw new ProductCodeIsDuplicatedExeption();
            }


            var categoryId = _categoryRepository.FindByName(dto.CategoryName).Id;

            var product = new Product
            {
                Name = dto.Name,
                CategoryId = categoryId,
                ProductCode = dto.ProductCode,
                MaxInStock = dto.MaxInStock,
                MinInStock = dto.MinInStock,
                BuyPrice = dto.BuyPrice,
                SellPrice = dto.SellPrice,
                Quantity = dto.Quantity,
            };
            _repository.Add(product);
            _unitOfWork.Commit();
        }
    }
}
