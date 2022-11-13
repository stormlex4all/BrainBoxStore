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
        private readonly ICartRepository _cartRepository;

        public CartProductHandler(ICartProductRepository cartProductRepository, ICoreValidationService<CartProduct> coreValidationService,
                                ICartRepository cartRepository) 
        {
            _cartProductRepository = cartProductRepository;
            _coreValidationService = coreValidationService;
            _cartRepository = cartRepository;
        }

        /// <summary>
        /// Create cartProduct record in db
        /// </summary>
        /// <param name="cartProduct"></param>
        /// <returns></returns>
        /// <exception cref="CartProductActionException"></exception>
        public async Task<CartProductDTO> CreateAsync(CartProductCreateDTO cartProduct)
        {
            string cartId = (await _cartRepository.GetByUserIdAsync(_cartProductRepository.GetTokenUserId())).Id;
            await _coreValidationService.ValidateRecordWithConditionDoesNotExist(c => c.Product.Id == cartProduct.ProductId && c.Cart.Id == cartId);
            string id = Guid.NewGuid().ToString();
            if (!await _cartProductRepository.AddAsync(new CartProduct
            {
                Id = id,
                CartId = cartId,
                ProductId = cartProduct.ProductId
            }))
            {
                throw new CartProductActionException(ResponseLang.CouldNotCreateRecord(nameof(CartProduct)));
            }

            return new() {Id = id, CartId = cartId, ProductId = cartProduct.ProductId };
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
        /// Delete product from cart using userId and productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="CartProductActionException"></exception>
        public async Task<bool> DeleteByProductIdAsync(string productId)
        {
            var cartProduct = await _cartProductRepository.GetAsync(c => c.Product.Id == productId && c.Cart.User.Id == _cartProductRepository.GetTokenUserId());
            if (cartProduct == null)
            {
                throw new CartProductActionException(ResponseLang.RecordNotFound());
            }
            return await _cartProductRepository.DeleteAsync(cartProduct);
        }

        /// <summary>
        /// Get all cart products using userId
        /// </summary>
        /// <param name="page"></param>
        /// <param name="recordsPerPage"></param>
        /// <returns></returns>
        public async Task<IList<CartProductDTO>> GetByUserIdAsync(int page, int recordsPerPage)
        {
            return await _cartProductRepository.GetAllAsync(c => c.Cart.User.Id == _cartProductRepository.GetTokenUserId(), page, recordsPerPage);
        }
    }
}
