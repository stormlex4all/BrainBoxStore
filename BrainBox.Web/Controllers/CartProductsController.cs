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
        public async Task<IActionResult> AddProductToCart(CartProductCreateDTO cartProduct)
        {
            try
            {
                StoreToken();
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
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = "This product has been added already!" });
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
        /// Get List of Products in a cart
        /// </summary>
        /// <param name="page"></param>
        /// <param name="recordsPerPage"></param>
        /// <returns></returns>
        [HttpGet("{page}/{recordsPerPage}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<IList<CartProductDTO>>))]
        public async Task<IActionResult> GetProductsInCart(int page = 0, int recordsPerPage = 10)
        {
            try
            {
                StoreToken();
                _logger.LogInformation($"Trying to GetProductsInCart");
                return Ok(new APIResponse<IList<CartProductDTO>> { ResponseObject = await _cartProductHandler.GetByUserIdAsync(page, recordsPerPage) });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"GetProductsInCart exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }

        /// <summary>
        /// Get cart with all products in the cart for user
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<CartDTO>))]
        public async Task<IActionResult> GetProductsInCart()
        {
            try
            {
                StoreToken();
                _logger.LogInformation($"Trying to GetProductsInCart");
                return Ok(new APIResponse<CartDTO> { ResponseObject = await _cartProductHandler.GetByUserIdAsync() });
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
                StoreToken();
                _logger.LogInformation($"Trying to DeleteProductFromCart, id: {cartProductId}");
                if (await _cartProductHandler.DeleteAsync(cartProductId))
                {
                    return Ok(new APIResponse<string> { ResponseObject = ResponseLang.SuccessfullyDeleted() });
                }
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.NotDeleted() });
            }
            catch (CartProductActionException exception)
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
        /// Delete product from cart using productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("product/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<string>))]
        public async Task<IActionResult> DeleteProductByProductIdFromCart(string productId)
        {
            try
            {
                StoreToken();
                _logger.LogInformation($"Trying to DeleteProductFromCart productId: {productId}");
                if (await _cartProductHandler.DeleteByProductIdAsync(productId))
                {
                    return Ok(new APIResponse<string> { ResponseObject = ResponseLang.SuccessfullyDeleted() });
                }
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.NotDeleted() });
            }
            catch (CartProductActionException exception)
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
