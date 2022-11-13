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
        Task<CartProductDTO> CreateAsync(CartProductCreateDTO cartProduct);

        /// <summary>
        /// Get all cart products using userId
        /// </summary>
        /// <param name="page"></param>
        /// <param name="recordsPerPage"></param>
        /// <returns></returns>
        Task<IList<CartProductDTO>> GetByUserIdAsync(int page, int recordsPerPage);

        /// <summary>
        /// Delete Delete product from cart using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// Delete product from cart using userId and productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="CartProductActionException"></exception>
        Task<bool> DeleteByProductIdAsync(string productId);

        /// <summary>
        /// Get all products in cart for user
        /// </summary>
        /// <returns></returns>
        Task<CartDTO> GetByUserIdAsync();
    }
}
