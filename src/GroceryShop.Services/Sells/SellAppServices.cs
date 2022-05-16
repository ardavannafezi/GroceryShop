using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Products;
using GroceryShop.Services.Products.Contracts;
using GroceryShop.Services.Sells.Contract;
using GroceryShop.Services.Sells.Contracts;
using System.Collections.Generic;

namespace GroceryShop.Services.Imports
{
    public class SellAppServices : SellServices
    {
        private readonly SellRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public SellAppServices(
            SellRepository repository,
            UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public void Add(AddSellDto dto)
        {

            bool isProductCodeExist = _repository.isProductCodeExist(dto.ProductCode);
            if (!isProductCodeExist)
            {
                throw new ProductNotFoundExeption();
            }

            Product product = _repository.FindProductById(dto.ProductCode);

            if(product.Quantity < dto.Quantity) { 

                throw new NotEcoughtInStock();
            }
            product.Quantity = product.Quantity - dto.Quantity;
            _repository.UpdateProduct(product);


            var sell = new Sell
            {
                ProductCode = dto.ProductCode,
                Quantity = dto.Quantity,
            };
            _repository.Add(sell);
            _unitOfWork.Commit();


            if (product.Quantity <= _repository
                .GetProductMaxInStock(dto.ProductCode))
            {
                new ReachedMinimumInStockAlert();
            }
        }
        
        public void Delete(int id)
        {
            if (!_repository.isExist(id))
            {
                throw new SellNotFoundExeption();
            };

            Sell sell = _repository.GetById(id);

            Product product = _repository
                .FindProductById(sell.ProductCode);

            product.Quantity = product.Quantity + sell.Quantity;

            _repository.UpdateProduct(product);

            _repository.Delete(sell);
            _unitOfWork.Commit();
        }

        public List<GetSellsDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(UpdateSellDto dto, int id)
        {
            if (!_repository.isExist(id))
            {
                throw new SellNotFoundExeption();
            };

            Product product = _repository
               .FindProductById(_repository.GetById(id).ProductCode);

            product.Quantity = product.Quantity
                + _repository.GetById(id).Quantity - dto.Quantity;

            _repository.UpdateProduct(product);


            Sell sell = _repository.GetById(id);

            sell.ProductCode = dto.ProductCode;
            sell.Quantity = dto.Quantity;

            _repository.Update(sell);
            _unitOfWork.Commit();
        }
    }
}

