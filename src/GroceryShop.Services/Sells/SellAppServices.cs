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
        private readonly ProductRepository _productRepository;

        private readonly SellRepository _repository;

        private readonly UnitOfWork _unitOfWork;

        private readonly CategoryRepository _categoryRepository;


        public SellAppServices(

            SellRepository repository,
            UnitOfWork unitOfWork,
            CategoryRepository cateogryRepository,
            ProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _categoryRepository = cateogryRepository;
            _productRepository = productRepository;
        }

        public void Add(AddSellDto dto)
        {

            bool isProductCodeExist = _productRepository.isProductCodeExist(dto.ProductCode);
            if (!isProductCodeExist)
            {
                throw new ProductNotFoundExeption();
            }

            Product product = _productRepository.FindById(dto.ProductCode);

            if(product.Quantity < dto.Quantity) { 

                throw new NotEcoughtInStock();
            }
            product.Quantity = product.Quantity - dto.Quantity;
            _productRepository.Update(product);


            var sell = new Sell
            {
                ProductCode = dto.ProductCode,
                Quantity = dto.Quantity,
            };
            _repository.Add(sell);
            _unitOfWork.Commit();

            if (product.Quantity <= _productRepository
                .GetMaxInStock(dto.ProductCode))
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

            Product product = _productRepository
                .FindById(_repository.GetById(id).ProductCode);

            product.Quantity = product.Quantity + _repository.GetById(id).Quantity;

            _productRepository.Update(product);

            _repository.Delete(id);
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

            Product product = _productRepository
               .FindById(_repository.GetById(id).ProductCode);

            product.Quantity = product.Quantity
                + _repository.GetById(id).Quantity - dto.Quantity;

            _productRepository.Update(product);


            Sell sell = _repository.GetById(id);

            sell.ProductCode = dto.ProductCode;
            sell.Quantity = dto.Quantity;

            _repository.Update(sell);
            _unitOfWork.Commit();
        }
    }
}

