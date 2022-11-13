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
    public class ProductsController : BaseController
    {
        private readonly IProductHandler _productHandler;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductHandler productHandler, ILogger<ProductsController> logger,
                                        ICurrentActiveToken currentActiveToken) : base(currentActiveToken)
        {
            _productHandler = productHandler;
            _logger = logger;
        }

        /// <summary>
        /// Creates new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<ProductDTO>))]
        public async Task<IActionResult> CreateProduct(ProductCreateDTO product)
        {
            try
            {
                StoreToken();
                _logger.LogInformation($"Trying to CreateProduct: {JsonConvert.SerializeObject(product)}");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ValidateModelState());
                }
                return Ok(new APIResponse<ProductDTO> { ResponseObject = await _productHandler.CreateAsync(product) });
            }
            catch (SimilarRecordExistsException exception)
            {
                _logger.LogError(exception, string.Format($"CreateProduct exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = exception.Message });
            }
            catch (ProductActionException exception)
            {
                _logger.LogError(exception, string.Format($"CreateProduct exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = exception.Message });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"CreateProduct exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }

        /// <summary>
        /// Get All Products
        /// </summary>
        /// <param name="page"></param>
        /// <param name="recordsPerPage"></param>
        /// <returns></returns>
        [HttpGet("{page}/{recordsPerPage}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<IList<ProductDTO>>))]
        public async Task<IActionResult> GetAllProducts(int page = 0, int recordsPerPage = 10)
        {
            try
            {
                StoreToken();
                _logger.LogInformation($"Trying to GetAllProducts:");
                return Ok(new APIResponse<IList<ProductDTO>> { ResponseObject = await _productHandler.GetAllAsync(page, recordsPerPage) });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"GetAllProducts exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }

        /// <summary>
        /// Search for Products
        /// </summary>
        /// <param name="productSearch"></param>
        /// <returns></returns>
        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<IList<ProductDTO>>))]
        public async Task<IActionResult> SearchProducts(ProductSearchDTO productSearch)
        {
            try
            {
                StoreToken();
                _logger.LogInformation($"Trying to SearchProducts: {JsonConvert.SerializeObject(productSearch)}");
                return Ok(new APIResponse<IList<ProductDTO>> { ResponseObject = await _productHandler.GetAllAsync(productSearch) });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"SearchProducts exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }

        /// <summary>
        /// Get Product using product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<ProductDTO>))]
        public async Task<IActionResult> GetProductById(string id)
        {
            try
            {
                StoreToken();
                _logger.LogInformation($"Trying to GetProductById, id: {id}");
                return Ok(new APIResponse<ProductDTO> { ResponseObject = await _productHandler.GetByIdAsync(id) });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"GetProductById exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }

        /// <summary>
        /// Updates the product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPut("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<string>))]
        public async Task<IActionResult> UpdateProduct(ProductCreateDTO product, string productId)
        {
            try
            {
                StoreToken();
                product.Id = productId;
                _logger.LogInformation($"Trying to UpdateProduct: {JsonConvert.SerializeObject(product)}");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ValidateModelState());
                }
                if (await _productHandler.UpdateAsync(product))
                {
                    return Ok(new APIResponse<string> { ResponseObject = ResponseLang.SuccessfullyUpdated() });
                }
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.NotUpdated() });
            }
            catch (ProductActionException exception)
            {
                _logger.LogError(exception, string.Format($"UpdateProduct exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = exception.Message });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"UpdateProduct exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }

        /// <summary>
        /// Deletes the product using the productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<string>))]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            try
            {
                StoreToken();
                _logger.LogInformation($"Trying to DeleteProduct, id: {productId}");
                if (await _productHandler.DeleteAsync(productId))
                {
                    return Ok(new APIResponse<string> { ResponseObject = ResponseLang.SuccessfullyDeleted() });
                }
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.NotDeleted() });
            }
            catch (ProductActionException exception)
            {
                _logger.LogError(exception, string.Format($"DeleteProduct exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = exception.Message });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"DeleteProduct exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }
    }
}
