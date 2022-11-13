using BrainBox.Data;
using BrainBox.Data.DTOs;
using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Models;
using BrainBox.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        public async Task<List<ProductDTO>> GetAllRecords(int page, int recordsPerPage)
        {
            return await _dbSet.Where(r => !r.IsDeleted).Select(c => new ProductDTO
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy,
                Price = c.Price,
                ProductCategory = new ProductCategoryDTO { Id = c.ProductCategoryId, Name = c.ProductCategory.Name },
            }).Skip(page).Take(recordsPerPage).ToListAsync();
        }

        /// <summary>
        /// Get all the product records as a list according to search params
        /// </summary>
        /// <param name="productSearch"></param>
        /// <returns></returns>
        public async Task<List<ProductDTO>> GetAllProductsAsync(ProductSearchDTO productSearch)
        {
            return await _dbSet.Where(r => 
                (!string.IsNullOrEmpty(productSearch.ProductId) && (r.Id == productSearch.ProductId)) ||
                (!string.IsNullOrEmpty(productSearch.ProductName) && r.Name.Contains(productSearch.ProductName.Trim())) ||
                (!string.IsNullOrEmpty(productSearch.ProductCategoryId) && (r.ProductCategory.Id == productSearch.ProductCategoryId)) ||
                (!string.IsNullOrEmpty(productSearch.ProductCategoryName) && r.ProductCategory.Name.Contains(productSearch.ProductCategoryName.Trim()))
            ).Select(c => new ProductDTO
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy,
                Price = c.Price,
                ProductCategory = new ProductCategoryDTO { Id = c.ProductCategoryId, Name = c.ProductCategory.Name },
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
                CreatedBy = c.CreatedBy,
                Price = c.Price,
                ProductCategory = new ProductCategoryDTO { Id = c.ProductCategoryId, Name = c.ProductCategory.Name },
            }).FirstOrDefaultAsync();
        }
    }
}
