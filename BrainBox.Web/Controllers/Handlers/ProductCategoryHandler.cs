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
    public class ProductCategoryHandler : IProductCategoryHandler
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ICoreValidationService<ProductCategory> _coreValidationService;
        public ProductCategoryHandler(IProductCategoryRepository productCategoryRepository, ICoreValidationService<ProductCategory> coreValidationService)
        {
            _productCategoryRepository = productCategoryRepository;
            _coreValidationService = coreValidationService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        /// <exception cref="ProductCategoryActionException"></exception>
        public async Task<ProductCategoryDTO> CreateAsync(ProductCategoryCreateDTO productCategory)
        {
            await _coreValidationService.ValidateRecordWithConditionDoesNotExist(c => c.Name.ToLower() == productCategory.Name.ToLower().Trim());
            string id = Guid.NewGuid().ToString();
            var category = new ProductCategory
            {
                Name = productCategory.Name,
                CreatedAt = DateTime.Now,
                Id = id
            };
            if (!await _productCategoryRepository.AddAsync(category))
            {
                throw new ProductCategoryActionException(ResponseLang.CouldNotCreateRecord(nameof(ProductCategory)));
            }
            productCategory.Id = id;
            return new() { Id = category.Id, Name = category.Name, CreatedAt = category.CreatedAt};
        }

        /// <summary>
        /// Deletes the product category record with Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ProductCategoryActionException"></exception>
        public async Task<bool> DeleteAsync(string id)
        {
            var productCategory = await _productCategoryRepository.GetAsync(id);
            if (productCategory == null)
            {
                throw new ProductCategoryActionException(ResponseLang.RecordNotFound());
            }
            return await _productCategoryRepository.DeleteAsync(productCategory);
        }

        /// <summary>
        /// Get all product category records
        /// </summary>
        /// <returns></returns>
        public async Task<IList<ProductCategoryDTO>> GetAllAsync(int page, int recordsPerPage)
        {
            return await _productCategoryRepository.GetAllRecords(page, recordsPerPage);
        }

        /// <summary>
        /// Get all the ProductCategory records as a list according to predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IList<ProductCategoryDTO>> GetAllAsync(Expression<Func<ProductCategory, bool>> predicate)
        {
            return await _productCategoryRepository.GetAllProductCategoriesAsync(predicate);
        }

        /// <summary>
        /// Get the ProductCategory with the given Id
        /// </summary>
        /// <param name="id">Int64</param>
        /// <returns>TEntity</returns>
        public async Task<ProductCategoryDTO> GetByIdAsync(string id)
        {
            return await _productCategoryRepository.GetProductCategoryAsync(id);
        }

        /// <summary>
        /// Updates the product category in the DB
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        /// <exception cref="ProductCategoryActionException"></exception>
        public async Task<bool> UpdateAsync(ProductCategoryCreateDTO productCategory)
        {
            await _coreValidationService.ValidateRecordWithConditionDoesNotExist(c => c.Name.ToLower() == productCategory.Name.ToLower().Trim());
            var category = await _productCategoryRepository.GetAsync(productCategory.Id);
            if(category == null)
            {
                throw new ProductCategoryActionException(ResponseLang.RecordNotFound());
            }
            category.Name = productCategory.Name;
            return await _productCategoryRepository.UpdateAsync(category);
        }
    }
}
