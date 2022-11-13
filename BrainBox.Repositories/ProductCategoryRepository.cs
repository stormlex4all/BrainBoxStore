using BrainBox.Data;
using BrainBox.Data.DTOs;
using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Models;
using BrainBox.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BrainBox.Repositories
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(AppDbContext appDBContext, ICurrentActiveToken currentActiveToken) : base(appDBContext, currentActiveToken)
        {
        }

        /// <summary>
        /// Get all product category records
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductCategoryDTO>> GetAllRecords(int page, int recordsPerPage)
        {
            return await _dbSet.Where(r => !r.IsDeleted).Select(c => new ProductCategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy
            }).Skip(page).Take(recordsPerPage).ToListAsync();
        }

        /// <summary>
        /// Get all the records of type T as a list according to predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<ProductCategoryDTO>> GetAllProductCategoriesAsync(Expression<Func<ProductCategory, bool>> predicate)
        {
            return await _dbSet.Where(r => !r.IsDeleted).Where(predicate).Select(c => new ProductCategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy
            }).ToListAsync();
        }

        /// <summary>
        /// Get the entity with the given Id
        /// </summary>
        /// <param name="id">Int64</param>
        /// <returns>TEntity</returns>
        public async Task<ProductCategoryDTO> GetProductCategoryAsync(string id)
        {
            return await _dbSet.Where(r => !r.IsDeleted && r.Id == id).Select(c => new ProductCategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy
            }).FirstOrDefaultAsync();
        }
    }
}
