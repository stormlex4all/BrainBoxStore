using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Models;
using System.Linq.Expressions;

namespace BrainBox.Repositories.Contracts
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        /// <summary>
        /// Get all product category records
        /// </summary>
        /// <returns></returns>
        Task<List<ProductCategoryDTO>> GetAllRecords();

        /// <summary>
        /// Get all the records of type T as a list according to predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<ProductCategoryDTO>> GetAllProductCategoriesAsync(Expression<Func<ProductCategory, bool>> predicate);

        /// <summary>
        /// Get the entity with the given Id
        /// </summary>
        /// <param name="id">Int64</param>
        /// <returns>TEntity</returns>
        Task<ProductCategoryDTO> GetProductCategoryAsync(string id);
    }
}
