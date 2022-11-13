using BrainBox.Core.Exceptions;
using BrainBox.Core.Lang;
using BrainBox.Data.DTOs;
using BrainBox.Data.DTOs.Store;
using BrainBox.Web.Controllers.Handlers.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BrainBox.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResponse<string>))]
    [Route("v1/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class CartProductsController : BaseController
    {
        private readonly ICartProductHandler _cartProductHandler;
        private readonly ILogger<CartProductsController> _logger;

        public CartProductsController(ICartProductHandler cartProductHandler, ILogger<CartProductsController> logger,
                                        ICurrentActiveToken currentActiveToken) : base(currentActiveToken)
        {
            _cartProductHandler = cartProductHandler;
            _logger = logger;
        }

        /// <summary>
        /// Adds product to the cart
        /// </summary>
        /// <param name="cartProduct"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<CartProductDTO>))]
        [Route("add-product")]
        public async Task<IActionResult> AddProductToCart(CartProductDTO cartProduct)
        {
            try
            {
                _logger.LogInformation($"Trying to AddProductToCart: {JsonConvert.SerializeObject(cartProduct)}");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ValidateModelState());
                }
                return Ok(new APIResponse<CartProductDTO> { ResponseObject = await _cartProductHandler.CreateAsync(cartProduct) });
            }
            catch (SimilarRecordExistsException exception)
            {
                _logger.LogError(exception, string.Format($"AddProductToCart exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = exception.Message });
            }
            catch (CartProductActionException exception)
            {
                _logger.LogError(exception, string.Format($"AddProductToCart exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = exception.Message });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"AddProductToCart exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }

        /// <summary>
        /// Get Products in a cart using user Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<IList<CartProductDTO>>))]
        public async Task<IActionResult> GetProductsInCart(string userId)
        {
            try
            {
                _logger.LogInformation($"Trying to GetProductsInCart, id: {userId}");
                return Ok(new APIResponse<IList<CartProductDTO>> { ResponseObject = await _cartProductHandler.GetByUserIdAsync(userId) });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"GetProductsInCart exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }

        /// <summary>
        /// Removes the product from the cart using the cartProductId
        /// </summary>
        /// <param name="cartProductId"></param>
        /// <returns></returns>
        [HttpDelete("{cartProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<string>))]
        public async Task<IActionResult> DeleteProductFromCart(string cartProductId)
        {
            try
            {
                _logger.LogInformation($"Trying to DeleteProductFromCart, id: {cartProductId}");
                if (await _cartProductHandler.DeleteAsync(cartProductId))
                {
                    return Ok(new APIResponse<string> { ResponseObject = ResponseLang.SuccessfullyDeleted() });
                }
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.NotDeleted() });
            }
            catch (ProductActionException exception)
            {
                _logger.LogError(exception, string.Format($"DeleteProductFromCart exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = exception.Message });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"DeleteProductFromCart exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }

        /// <summary>
        /// Delete product from cart using userId and productId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("{userId}/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<string>))]
        public async Task<IActionResult> DeleteProductFromCart(string userId, string productId)
        {
            try
            {
                _logger.LogInformation($"Trying to DeleteProductFromCart, userId: {userId}, productId: {productId}");
                if (await _cartProductHandler.DeleteAsync(userId, productId))
                {
                    return Ok(new APIResponse<string> { ResponseObject = ResponseLang.SuccessfullyDeleted() });
                }
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.NotDeleted() });
            }
            catch (ProductActionException exception)
            {
                _logger.LogError(exception, string.Format($"DeleteProductFromCart exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = exception.Message });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"DeleteProductFromCart exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }
    }
}
