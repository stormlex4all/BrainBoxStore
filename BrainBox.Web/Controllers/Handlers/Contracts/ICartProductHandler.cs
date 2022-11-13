using BrainBox.Core.Exceptions;
using BrainBox.Data.DTOs.Store;

namespace BrainBox.Web.Controllers.Handlers.Contracts
{
    public interface ICartProductHandler
    {
        /// <summary>
        /// Create cartProduct record in db
        /// </summary>
        /// <param name="cartProduct"></param>
        /// <returns></returns>
        /// <exception cref="CartProductActionException"></exception>
        Task<CartProductDTO> CreateAsync(CartProductDTO cartProduct);

        /// <summary>
        /// Get all cart products using userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IList<CartProductDTO>> GetByUserIdAsync(string userId);

        /// <summary>
        /// Delete Delete product from cart using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// Delete product from cart using userId and productId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="CartProductActionException"></exception>
        Task<bool> DeleteAsync(string userId, string productId);
    }
}
