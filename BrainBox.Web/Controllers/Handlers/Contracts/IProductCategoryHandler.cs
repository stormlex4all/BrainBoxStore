using BrainBox.Core.Exceptions;
using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Models;
using System.Linq.Expressions;

namespace BrainBox.Web.Controllers.Handlers.Contracts
{
    public interface IProductCategoryHandler
    {
        /// <summary>
        /// Adds new productCategory record to the DB
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        Task<ProductCategoryDTO> CreateAsync(ProductCategoryCreateDTO productCategory);

        /// <summary>
        /// Get record by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductCategoryDTO> GetByIdAsync(string id);

        /// <summary>
        /// Gets All the ProductCategory records from DB
        /// </summary>
        /// <returns></returns>
        Task<IList<ProductCategoryDTO>> GetAllAsync();

        /// <summary>
        ///  Gets All the ProductCategory records from DB using an expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IList<ProductCategoryDTO>> GetAllAsync(Expression<Func<ProductCategory, bool>> predicate);

        /// <summary>
        /// Updates the category in the DB
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        /// <exception cref="ProductCategoryActionException"></exception>
        Task<bool> UpdateAsync(ProductCategoryCreateDTO productCategory);

        /// <summary>
        /// Delete single record from db using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string id);
    }
}
