using BrainBox.Core.CoreServices.Contracts;
using BrainBox.Core.Exceptions;
using BrainBox.Core.Lang;
using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Models;
using BrainBox.Repositories;
using BrainBox.Repositories.Contracts;
using BrainBox.Web.Controllers.Handlers.Contracts;

namespace BrainBox.Web.Controllers.Handlers
{
    public class CartHandler : ICartHandler
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICoreValidationService<Cart> _coreValidationService;

        public CartHandler(ICartRepository cartRepository, ICoreValidationService<Cart> coreValidationService)
        {
            _cartRepository = cartRepository;
            _coreValidationService = coreValidationService;
        }

        /// <summary>
        /// Adds new cart record to the DB
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        /// <exception cref="CartActionException"></exception>
        public async Task<CartDTO> CreateAsync(CartDTO cart)
        {
            await _coreValidationService.ValidateRecordWithConditionDoesNotExist(c => c.User.Id == cart.User.Id, true);
            string id = Guid.NewGuid().ToString();
            if (!await _cartRepository.AddAsync(new Cart
            {
                UserId = cart.User.Id,
                Id = id,
                CreatedBy = cart.User.Id
            }))
            {
                throw new CartActionException(ResponseLang.CouldNotCreateRecord(nameof(Cart)));
            }
            cart.Id = id;
            return cart;
        }

        /// <summary>
        /// Delete cart record from db using userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="CartActionException"></exception>
        public async Task<bool> DeleteAsync(string userId)
        {
            var cart = await _cartRepository.GetAsync(c => c.User.Id == userId);
            if (cart == null)
            {
                throw new CartActionException(ResponseLang.RecordNotFound());
            }
            return await _cartRepository.DeleteAsync(cart);
        }

        /// <summary>
        /// Get cart and products in cart by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<CartDTO> GetByUserIdAsync(string userId)
        {
            return await _cartRepository.GetByUserIdAsync(userId);
        }
    }
}
