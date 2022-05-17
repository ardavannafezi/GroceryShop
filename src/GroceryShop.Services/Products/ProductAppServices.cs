using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Products.Contracts;
using System.Collections.Generic;

namespace GroceryShop.Services.Products
{
    public class ProductAppServices : ProductServices
    {
        private readonly ProductRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public ProductAppServices(
            ProductRepository repositoy,
            UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repositoy;
        }

        public void Add(AddProductDto dto)
        {
            bool isProductNameExist = _repository.isProductNameExistInCategory
                (dto.Name,dto.CategoryId);
            if (isProductNameExist)
            {
                throw new ProductNameIsDuplicatedExeption();
            }

            bool isProductCodeExist = _repository.isProductCodeExist(dto.ProductCode);
            if (isProductCodeExist)
            {
                throw new ProductCodeIsDuplicatedExeption();
            }

            var product = new Product
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                ProductCode = dto.ProductCode,
                MaxInStock = dto.MaxInStock,
                MinInStock = dto.MinInStock,
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

            Product product = _repository.FindById(code);

            _repository.Delete(product);
            _unitOfWork.Commit();
        }

        public IList<GetProductDto> GetAll()
        {
                return _repository.GetAll();
        }

        public void Update(UpdateProductDto dto, int id)
        {

            string productName = _repository.GetNameByCode(id);

            bool isProductNameAlreadyExist =
                _repository.isProductNameExistInCategory(dto.Name, dto.CategoryId);
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

            product.Name = dto.Name;
            product.Quantity = dto.Quantity;
            product.CategoryId = dto.CategoryId;
            product.ProductCode = dto.ProductCode;
            product.MaxInStock = dto.MaxInStock;
            product.MinInStock = dto.MinInStock;

            _repository.Update(product);
            _unitOfWork.Commit();
        }
    }
}
