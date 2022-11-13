using BrainBox.Data;
using BrainBox.Data.DTOs;
using BrainBox.Data.DTOs.Auth;
using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Models;
using BrainBox.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BrainBox.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext appDBContext, ICurrentActiveToken currentActiveToken) : base(appDBContext, currentActiveToken)
        {
        }

        /// <summary>
        /// Get cart and products in cart by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<CartDTO> GetByUserIdAsync(string userId)
        {
            return await _dbSet.Where(c => c.User.Id == userId && !c.IsDeleted).Select(c => new CartDTO() 
            {
                Id = c.Id,
                User = new UserDTO { Id = c.User.Id, Email = c.User.Email},
                CartProducts = c.CartProducts.Select(c => new CartProductDTO() { Id = c.Id, ProductName = c.Product.Name, ProductAmount = c.Product.Price, ProductCategoryName = c.Product.ProductCategory.Name }).ToList(),
            }).FirstOrDefaultAsync();
        }
    }
}
