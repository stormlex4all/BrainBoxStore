using BrainBox.Data;
using BrainBox.Data.DTOs;
using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Models;
using BrainBox.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BrainBox.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext appDBContext, ICurrentActiveToken currentActiveToken) : base(appDBContext, currentActiveToken)
        {
        }

        /// <summary>
        /// Get all product records
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductDTO>> GetAllRecords()
        {
            return await _dbSet.Where(r => !r.IsDeleted).Select(c => new ProductDTO
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy
            }).ToListAsync();
        }

        /// <summary>
        /// Get all the product records as a list according to search params
        /// </summary>
        /// <param name="productSearch"></param>
        /// <returns></returns>
        public async Task<List<ProductDTO>> GetAllProductsAsync(ProductSearchDTO productSearch)
        {
            return await _dbSet.Where(r => 
                (productSearch.ProductId != default && (r.Id == productSearch.ProductId)) ||
                (!string.IsNullOrEmpty(productSearch.ProductName.Trim()) && r.Name.Contains(productSearch.ProductName)) ||
                (productSearch.ProductCategoryId != default && (r.ProductCategory.Id == productSearch.ProductCategoryId)) ||
                (!string.IsNullOrEmpty(productSearch.ProductCategoryName.Trim()) && r.ProductCategory.Name.Contains(productSearch.ProductCategoryName))
            ).Select(c => new ProductDTO
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy
            }).ToListAsync();
        }

        /// <summary>
        /// Get the product with the given Id
        /// </summary>
        /// <param name="id">Int64</param>
        /// <returns>TEntity</returns>
        public async Task<ProductDTO> GetProductAsync(string id)
        {
            return await _dbSet.Where(r => !r.IsDeleted && r.Id == id).Select(c => new ProductDTO
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy
            }).FirstOrDefaultAsync();
        }
    }
}
