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

        public void Delete(int code)
        {
            if (!_repository.isCodeAlreadyExist(code))
            {
                throw new ProductNotFoundExeption();
            }
           

            _repository.Delete(_repository.FindById(code));
            _unitOfWork.Commit();
        }

        public IList<GetProductDto> GetAll()
        {
                return _repository.GetAll();
        }

        public void Update(UpdateProductDto dto, int id)
        {
            string productName = _repository.GetNameByCode(id);

            bool isProductNameAlreadyExist = _repository.isNameAlreadyExist(dto.Name);
            if (isProductNameAlreadyExist && productName != dto.Name)
            {
                throw new ProductNameIsDuplicatedExeption();
            }

            bool isProductCodeAlreadyExist = _repository.isCodeAlreadyExist(dto.ProductCode);
            if (isProductCodeAlreadyExist && id != dto.ProductCode)
            {
                throw new ProductCodeIsDuplicatedExeption();
            }


            Product product = _repository.FindById(dto.ProductCode);

            var categoryId = _categoryRepository.FindByName(dto.CategoryName).Id;

            product.Name = dto.Name;
            product.BuyPrice = dto.BuyPrice;
            product.SellPrice = dto.SellPrice;
            product.Quantity = dto.Quantity;
            product.CategoryId = categoryId;
            product.ProductCode = dto.ProductCode;
            product.MaxInStock = dto.MaxInStock;
            product.MinInStock = dto.MinInStock;



            _repository.Update(product);
            _unitOfWork.Commit();

        }
    }
}
