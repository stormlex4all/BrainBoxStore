using BrainBox.Core.CoreServices.Contracts;
using BrainBox.Core.Exceptions;
using BrainBox.Core.Lang;
using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Models;
using BrainBox.Repositories.Contracts;
using BrainBox.Web.Controllers.Handlers.Contracts;

namespace BrainBox.Web.Controllers.Handlers
{
    public class CartProductHandler : ICartProductHandler
    {
        private readonly ICartProductRepository _cartProductRepository;
        private readonly ICoreValidationService<CartProduct> _coreValidationService;

        public CartProductHandler(ICartProductRepository cartProductRepository, ICoreValidationService<CartProduct> coreValidationService)
        {
            _cartProductRepository = cartProductRepository;
            _coreValidationService = coreValidationService;
        }

        /// <summary>
        /// Create cartProduct record in db
        /// </summary>
        /// <param name="cartProduct"></param>
        /// <returns></returns>
        /// <exception cref="CartProductActionException"></exception>
        public async Task<CartProductDTO> CreateAsync(CartProductDTO cartProduct)
        {
            await _coreValidationService.ValidateRecordWithConditionDoesNotExist(c => c.Product.Id == cartProduct.ProductId && c.Cart.Id == cartProduct.CartId);
            string id = Guid.NewGuid().ToString();
            if (!await _cartProductRepository.AddAsync(new CartProduct
            {
                Id = id,
                CartId = cartProduct.CartId,
                ProductId = cartProduct.ProductId
            }))
            {
                throw new CartProductActionException(ResponseLang.CouldNotCreateRecord(nameof(CartProduct)));
            }
            cartProduct.Id = id;
            return cartProduct;
        }

        /// <summary>
        /// Delete cartProduct record from db using Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CartProductActionException"></exception>
        public async Task<bool> DeleteAsync(string id)
        {
            var cartProduct = await _cartProductRepository.GetAsync(id);
            if (cartProduct == null)
            {
                throw new CartProductActionException(ResponseLang.RecordNotFound());
            }
            return await _cartProductRepository.DeleteAsync(cartProduct);
        }

        /// <summary>
        /// Delete cartProduct record from db using userId and productId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="CartProductActionException"></exception>
        public async Task<bool> DeleteAsync(string userId, string productId)
        {
            var cartProduct = await _cartProductRepository.GetAsync(c => c.Product.Id == productId && c.Cart.User.Id == userId);
            if (cartProduct == null)
            {
                throw new CartProductActionException(ResponseLang.RecordNotFound());
            }
            return await _cartProductRepository.DeleteAsync(cartProduct);
        }

        /// <summary>
        /// Get all cart products using userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IList<CartProductDTO>> GetByUserIdAsync(string userId)
        {
            return await _cartProductRepository.GetAllAsync(c => c.Cart.User.Id == userId);
        }
    }
}
