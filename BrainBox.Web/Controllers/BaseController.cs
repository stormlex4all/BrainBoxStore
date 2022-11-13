using BrainBox.Data.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BrainBox.Web.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        private readonly ICurrentActiveToken _currentActiveToken;

        public BaseController(ICurrentActiveToken currentActiveToken)
        {
            _currentActiveToken = currentActiveToken;
        }

        /// <summary>
        /// Validates the model state
        /// </summary>
        /// <returns></returns>
        protected APIResponse<string> ValidateModelState()
        {
            string errorMessage = string.Empty;
            foreach (var item in ModelState.Keys)
            {
                var modelState = ModelState[item];
                if (modelState.Errors.Count > 0)
                {
                    errorMessage = modelState.Errors.FirstOrDefault().ErrorMessage;
                    break;
                }
            }
            return new APIResponse<string> { Error = true, ResponseObject = errorMessage };
        }


        /// <summary>
        /// Saves the user access token
        /// </summary>
        /// <param name="token"></param>
        protected void StoreToken()
        {
            _currentActiveToken.Token = ((string)Request.Headers.Authorization).Replace("Bearer", "").Trim();
        }
    }
}
