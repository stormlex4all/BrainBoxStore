using BrainBox.Core.Exceptions;
using BrainBox.Data.DTOs.Store;

namespace BrainBox.Web.Controllers.Handlers.Contracts
{
    public interface ICartHandler
    {
        /// <summary>
        /// Adds new cart record to the DB
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        /// <exception cref="CartActionException"></exception>
        Task<CartDTO> CreateAsync(CartDTO cart);

        /// <summary>
        /// Delete cart record from db using userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="CartActionException"></exception>
        Task<bool> DeleteAsync(string userId);

        /// <summary>
        /// Get record by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CartDTO> GetByUserIdAsync(string userId);
    }
}
