using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Models;

namespace BrainBox.Repositories.Contracts
{
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Get all product records
        /// </summary>
        /// <returns></returns>
        Task<List<ProductDTO>> GetAllRecords();

        /// <summary>
        /// Get all the product records as a list according to search params
        /// </summary>
        /// <param name="productSearch"></param>
        /// <returns></returns>
        Task<List<ProductDTO>> GetAllProductsAsync(ProductSearchDTO productSearch);

        /// <summary>
        /// Get the product with the given Id
        /// </summary>
        /// <param name="id">Int64</param>
        /// <returns>TEntity</returns>
        Task<ProductDTO> GetProductAsync(string id);
    }
}
