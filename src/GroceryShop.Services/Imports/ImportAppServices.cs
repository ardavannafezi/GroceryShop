using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Services.Categories.Contracts;
using GroceryShop.Services.Imports.Contract;
using GroceryShop.Services.Products;
using GroceryShop.Services.Products.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Imports
{
    public class ImportAppServices : ImportServices
    {
        private readonly ProductRepository _productRepository;

        private readonly ImportRepository _repository;

        private readonly UnitOfWork _unitOfWork;

        private readonly CategoryRepository _categoryRepository;


        public ImportAppServices(

            ImportRepository repository,
            UnitOfWork unitOfWork,
            CategoryRepository cateogryRepository,
            ProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _categoryRepository = cateogryRepository;
            _productRepository = productRepository;
        }

        public void Add(AddImportDto dto)
        {

            bool isProductCodeExist = _productRepository.isProductCodeExist(dto.ProductCode);
            if (!isProductCodeExist)
            {
                throw new ProductNotFoundExeption();
            }


            int InStock = _productRepository.FindById(dto.ProductCode).Quantity;
            if (InStock + dto.Quantity > _productRepository
                .GetMaxInStock(dto.ProductCode))
            {
                throw new ReachedMaximumAllowedInStockExeption();
            }


            Product product = _productRepository.FindById(dto.ProductCode);
            product.Quantity = product.Quantity + dto.Quantity;
            _productRepository.Update(product);


            var import = new Import
            { 
                ProductCode = dto.ProductCode,
                Price = dto.Price,
                Quantity = dto.Quantity ,
            };

            _repository.Add(import);


            

            _unitOfWork.Commit();
        }

      
    }
}
