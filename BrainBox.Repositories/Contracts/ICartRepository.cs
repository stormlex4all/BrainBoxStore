using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Models;

namespace BrainBox.Repositories.Contracts
{
    public interface ICartRepository : IRepository<Cart>
    {
        /// <summary>
        /// Get cart and products in cart by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CartDTO> GetByUserIdAsync(string userId);
    }
}
