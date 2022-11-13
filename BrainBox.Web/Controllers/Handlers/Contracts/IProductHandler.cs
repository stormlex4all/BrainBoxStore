using BrainBox.Core.Exceptions;
using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Models;
using System.Linq.Expressions;

namespace BrainBox.Web.Controllers.Handlers.Contracts
{
    public interface IProductHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        /// <exception cref="ProductActionException"></exception>
        Task<ProductDTO> CreateAsync(ProductCreateDTO product);

        /// <summary>
        /// Get record by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductDTO> GetByIdAsync(string id);

        /// <summary>
        /// Get all product records
        /// </summary>
        /// <returns></returns>
        Task<IList<ProductDTO>> GetAllAsync(int page, int recordsPerPage);

        /// <summary>
        /// Get all the product records as a list according to search params
        /// </summary>
        /// <param name="productSearch"></param>
        /// <returns></returns>
        Task<IList<ProductDTO>> GetAllAsync(ProductSearchDTO productSearch);

        /// <summary>
        /// Updates the Product record in the DB
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(ProductCreateDTO product);

        /// <summary>
        /// Deletes the product record using the Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ProductActionException"></exception>
        Task<bool> DeleteAsync(string id);
    }
}
