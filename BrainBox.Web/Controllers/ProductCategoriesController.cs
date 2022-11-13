using BrainBox.Core.Exceptions;
using BrainBox.Core.Lang;
using BrainBox.Data.DTOs;
using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Models;
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
    public class ProductCategoriesController : BaseController
    {
        private readonly IProductCategoryHandler _productCategoryHandler;
        private readonly ILogger<ProductCategoriesController> _logger;

        public ProductCategoriesController(IProductCategoryHandler productCategoryHandler, ILogger<ProductCategoriesController> logger,
                                        ICurrentActiveToken currentActiveToken) : base(currentActiveToken)
        {
            _productCategoryHandler = productCategoryHandler;
            _logger = logger;
        }

        /// <summary>
        /// Creates new productCategory
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<ProductCategoryDTO>))]
        [Route("create")]
        public async Task<IActionResult> CreateProductCategory(ProductCategoryCreateDTO productCategory)
        {
            try
            {
                _logger.LogInformation($"Trying to CreateProductCategory: {JsonConvert.SerializeObject(productCategory)}");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ValidateModelState());
                }
                return Ok(new APIResponse<ProductCategoryDTO> { ResponseObject = await _productCategoryHandler.CreateAsync(productCategory) });
            }
            catch (SimilarRecordExistsException exception)
            {
                _logger.LogError(exception, string.Format($"CreateProductCategory exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = exception.Message });
            }
            catch (ProductCategoryActionException exception)
            {
                _logger.LogError(exception, string.Format($"CreateProductCategory exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = exception.Message });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"CreateProductCategory exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }

        /// <summary>
        /// Get All Product Categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<IList<ProductCategoryDTO>>))]
        public async Task<IActionResult> GetAllProductCategories()
        {
            try
            {
                _logger.LogInformation($"Trying to GetAllProductCategories:");
                return Ok(new APIResponse<IList<ProductCategoryDTO>> { ResponseObject = await _productCategoryHandler.GetAllAsync() });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"GetAllProductCategories exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }

        /// <summary>
        /// Get Product Category using product category Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<ProductCategoryDTO>))]
        [Route("{id}")]
        public async Task<IActionResult> GetProductCategoryById(string id)
        {
            try
            {
                _logger.LogInformation($"Trying to GetProductCategoryById, id: {id}");
                return Ok(new APIResponse<ProductCategoryDTO> { ResponseObject = await _productCategoryHandler.GetByIdAsync(id) });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"GetProductCategoryById exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }

        /// <summary>
        /// Updates the product category
        /// </summary>
        /// <param name="productCategory"></param>
        /// <param name="productCategoryId"></param>
        /// <returns></returns>
        [HttpPut("{productCategoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<string>))]
        public async Task<IActionResult> UpdateProductCategory(ProductCategoryCreateDTO productCategory, string productCategoryId)
        {
            try
            {
                productCategory.Id = productCategoryId;
                _logger.LogInformation($"Trying to UpdateProductCategory: {JsonConvert.SerializeObject(productCategory)}");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ValidateModelState());
                }
                if (await _productCategoryHandler.UpdateAsync(productCategory))
                {
                    return Ok(new APIResponse<string> { ResponseObject =  ResponseLang.SuccessfullyUpdated()});
                }
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.NotUpdated() });
            }
            catch (ProductCategoryActionException exception)
            {
                _logger.LogError(exception, string.Format($"UpdateProductCategory exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = exception.Message });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"UpdateProductCategory exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }

        /// <summary>
        /// Deletes the product category using the productCategoryId
        /// </summary>
        /// <param name="productCategoryId"></param>
        /// <returns></returns>
        [HttpDelete("{productCategoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<string>))]
        public async Task<IActionResult> DeleteProductCategory(string productCategoryId)
        {
            try
            {
                _logger.LogInformation($"Trying to DeleteProductCategory, id: {productCategoryId}");
                if (await _productCategoryHandler.DeleteAsync(productCategoryId))
                {
                    return Ok(new APIResponse<string> { ResponseObject = ResponseLang.SuccessfullyDeleted() });
                }
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.NotDeleted() });
            }
            catch (ProductCategoryActionException exception)
            {
                _logger.LogError(exception, string.Format($"DeleteProductCategory exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = exception.Message });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format($"DeleteProductCategory exception {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }
    }
}
