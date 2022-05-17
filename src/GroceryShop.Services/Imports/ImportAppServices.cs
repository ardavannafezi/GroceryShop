using GroceryShop.Entities;
using GroceryShop.Infrastructure.Application;
using GroceryShop.Services.Imports.Contract;
using GroceryShop.Services.Products;
using GroceryShop.Services.Products.Contracts;
using System.Collections.Generic;

namespace GroceryShop.Services.Imports
{
    public class ImportAppServices : ImportServices
    {
        private readonly ImportRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public ImportAppServices(
            ImportRepository repository,
            UnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public void Add(AddImportDto dto)
        {
            bool isProductCodeExist = _repository.isProductCodeExist(dto.ProductCode);
            if (!isProductCodeExist)
            {
                throw new ProductNotFoundExeption();
            }

            int InStock = _repository.FindProductById(dto.ProductCode).Quantity;
            if (InStock + dto.Quantity > _repository
                .GetMaxInStock(dto.ProductCode))
            {
                throw new ReachedMaximumAllowedInStockExeption();
            }

            Product product = _repository.FindProductById(dto.ProductCode);

            product.Quantity = product.Quantity + dto.Quantity;

            var import = new Import
            { 
                ProductCode = dto.ProductCode,
                Quantity = dto.Quantity ,
            };

            _repository.UpdateProduct(product);
            _repository.Add(import);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {

            if (!_repository.isExist(id))
            {
                throw new ImportNotFoundExeption();
            };

            Import import = _repository.GetById(id);
            Product product = _repository
                .FindProductById(import.ProductCode);

            product.Quantity = product.Quantity - import.Quantity;

            _repository.UpdateProduct(product);
            _repository.Delete(import);
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

            int ImportproductCode = _repository.GetById(id).ProductCode;
            Product product = _repository
               .FindProductById(ImportproductCode);
            Import import = _repository.GetById(id);

            int NewQuantity = product.Quantity - import.Quantity + dto.Quantity;

            product.Quantity = NewQuantity;

            import.ProductCode = dto.ProductCode;
            import.Quantity = dto.Quantity;

            _repository.UpdateProduct(product);
            _repository.Update(import);
            _unitOfWork.Commit();
        }
    }
}
