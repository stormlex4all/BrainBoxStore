using BrainBox.Data;
using BrainBox.Data.DTOs;
using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Models;
using BrainBox.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BrainBox.Repositories
{
    public class CartProductRepository : Repository<CartProduct>, ICartProductRepository
    {

        public CartProductRepository(AppDbContext appDBContext, ICurrentActiveToken currentActiveToken) : base(appDBContext, currentActiveToken)
        {
        }

        /// <summary>
        /// Get all cart products satisfying the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IList<CartProductDTO>> GetAllAsync(Expression<Func<CartProduct, bool>> predicate)
        {
            return await _dbSet.Where(c => !c.IsDeleted).Where(predicate).Select(c => new CartProductDTO
            {
                Id = c.Id,
                CartId = c.Cart.Id,
                ProductId = c.Product.Id,
                ProductName = c.Product.Name,
                ProductAmount = c.Product.Price
            }).ToListAsync();
        }
    }
}
