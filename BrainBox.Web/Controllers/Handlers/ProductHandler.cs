using BrainBox.Core.CoreServices.Contracts;
using BrainBox.Core.Exceptions;
using BrainBox.Core.Lang;
using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Models;
using BrainBox.Repositories.Contracts;
using BrainBox.Web.Controllers.Handlers.Contracts;
using System.Linq.Expressions;

namespace BrainBox.Web.Controllers.Handlers
{
    public class ProductHandler : IProductHandler
    {
        private readonly IProductRepository _productRepository;
        private readonly ICoreValidationService<Product> _coreValidationService;
        public ProductHandler(IProductRepository productRepository, ICoreValidationService<Product> coreValidationService)
        {
            _productRepository = productRepository;
            _coreValidationService = coreValidationService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        /// <exception cref="ProductActionException"></exception>
        public async Task<ProductDTO> CreateAsync(ProductCreateDTO product)
        {
            await _coreValidationService.ValidateRecordWithConditionDoesNotExist(c => c.Name.ToLower() == product.Name.ToLower().Trim());
            string id = Guid.NewGuid().ToString();
            if (!await _productRepository.AddAsync(new Product
            {
                Name = product.Name,
                CreatedAt = DateTime.Now,
                Id = id,
                Price = product.Price,
                ProductCategoryId = product.ProductCategoryId
            }))
            {
                throw new ProductActionException(ResponseLang.CouldNotCreateRecord(nameof(Product)));
            }
            return new() { Id = id, Name = product.Name, Price = product.Price, ProductCategory = new ProductCategoryDTO { Id = product.ProductCategoryId} };
        }

        /// <summary>
        /// Deletes the product record using the Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ProductActionException"></exception>
        public async Task<bool> DeleteAsync(string id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                throw new ProductActionException(ResponseLang.RecordNotFound());
            }
            return await _productRepository.DeleteAsync(product);
        }

        /// <summary>
        /// Get all product records
        /// </summary>
        /// <returns></returns>
        public async Task<IList<ProductDTO>> GetAllAsync(int page, int recordsPerPage)
        {
            return await _productRepository.GetAllRecords(page, recordsPerPage);
        }

        /// <summary>
        /// Get all the product records as a list according to search params
        /// </summary>
        /// <param name="productSearch"></param>
        /// <returns></returns>
        public async Task<IList<ProductDTO>> GetAllAsync(ProductSearchDTO productSearch)
        {
            return await _productRepository.GetAllProductsAsync(productSearch);
        }

        /// <summary>
        /// Get the product with the given Id
        /// </summary>
        /// <param name="id">Int64</param>
        /// <returns>TEntity</returns>
        public async Task<ProductDTO> GetByIdAsync(string id)
        {
            return await _productRepository.GetProductAsync(id);
        }

        /// <summary>
        /// Updates the product in the DB
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ProductCreateDTO product)
        {
            await _coreValidationService.ValidateRecordWithConditionDoesNotExist(c => c.Name.ToLower() == product.Name.ToLower().Trim() && c.Id != product.Id);
            var category = await _productRepository.GetAsync(product.Id);
            if (category == null)
            {
                throw new ProductActionException(ResponseLang.RecordNotFound());
            }
            return await _productRepository.UpdateAsync(category);
        }
    }
}
