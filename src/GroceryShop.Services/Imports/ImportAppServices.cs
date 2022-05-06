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
                Quantity = dto.Quantity ,
            };

            _repository.Add(import);


            

            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {

            if (!_repository.isExist(id))
            {
                throw new ImportNotFoundExeption();
            };

            Product product = _productRepository
                .FindById(_repository.GetById(id).ProductCode);

            product.Quantity = product.Quantity - _repository.GetById(id).Quantity;
                
                
            _productRepository.Update(product);

            var import =  _repository.GetById(id);
            _repository.Delete(id);
            _unitOfWork.Commit();
        }

        public List<GetImportsDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(UpdateImportDto dto, int id)
        {
            if (!_repository.isExist(id))
            {
                throw new ImportNotFoundExeption();
            };

            Product product = _productRepository
               .FindById(_repository.GetById(id).ProductCode);

            product.Quantity = product.Quantity 
                - _repository.GetById(id).Quantity + dto.Quantity;

            _productRepository.Update(product);


            Import import = _repository.GetById(id);

            import.ProductCode = dto.ProductCode;
            import.Quantity = dto.Quantity;

            _repository.Update(import);
            _unitOfWork.Commit();

        }
    }
}
