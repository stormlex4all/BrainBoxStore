using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using BrainBox.Web.Controllers.Handlers.Contracts;
using BrainBox.Core.Exceptions;
using BrainBox.Core.Lang;
using BrainBox.Data.Models;
using BrainBox.Data.DTOs;
using BrainBox.Data.DTOs.Auth;

namespace BrainBox.Web.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationHandler _authenticationHandler;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationHandler authenticationHandler, ILogger<AuthenticationController> logger,
                                        ICurrentActiveToken currentActiveToken) : base(currentActiveToken)
        {
            _authenticationHandler = authenticationHandler;
            _logger = logger;
        }

        /// <summary>
        /// Gets an access token for the user if credentials are valid
        /// </summary>
        /// <param name="signInDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<TokenDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResponse<string>))]
        [Route("signin-user")]
        public async Task<IActionResult> SignIn(SignInDTO signInDTO)
        {
            try
            {
                _logger.LogInformation($"Trying to sign in user: {signInDTO.Email}");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ValidateModelState());
                }
                return Ok(new APIResponse<TokenDTO> { ResponseObject = await _authenticationHandler.ProcessSignInRequest(signInDTO) });
            }
            catch (UserSignInException exception)
            {
                _logger.LogError(string.Format($"SignIn error: {exception.Message}"));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = exception.Message });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format("SignIn exception {0}", exception.Message));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }

        /// <summary>
        /// Get a new access token if refresh token and token are valid
        /// </summary>
        /// <param name="tokenRequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<TokenDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResponse<string>))]
        [Route("refresh-token")]
        public IActionResult RefreshToken(TokenRequestDTO tokenRequestModel)
        {

            RefreshToken refreshToken = null;
            UserDTO user = null;
            try
            {
                _logger.LogInformation($"Trying to RefreshToken");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ValidateModelState());
                }
                return Ok(new APIResponse<TokenDTO> { ResponseObject = _authenticationHandler.RefreshToken(out refreshToken, out user, tokenRequestModel) });
            }
            catch (SecurityTokenExpiredException e)
            {
                if (refreshToken.ExpireAt >= DateTime.Now)
                {
                    return Ok(new APIResponse<TokenDTO> { ResponseObject = _authenticationHandler.GenerateJwtToken(user, refreshToken.Token) });
                }
                return Ok(new APIResponse<TokenDTO> { ResponseObject = _authenticationHandler.GenerateJwtToken(user) });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format("RefreshToken exception {0}", exception.Message));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }

        /// <summary>
        /// Adds a new user to the DB if validations are OK
        /// </summary>
        /// <param name="signUpDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResponse<string>))]
        [Route("register-user")]
        public async Task<IActionResult> RegisterUser(SignUpDTO signUpDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ValidateModelState());
                }

                _logger.LogInformation($"Trying to RegisterUser user: {JsonConvert.SerializeObject(signUpDTO)}");
                if (await _authenticationHandler.RegisterUser(signUpDTO))
                {
                    return Ok(new APIResponse<string> { ResponseObject = $"User {signUpDTO.Email} Successfully created" });
                }
                return BadRequest(new APIResponse<string> { ResponseObject = $"{ResponseLang.Genericexception()}" });
            }
            catch (CartActionException exception)
            {
                _logger.LogError(exception, string.Format("RegisterUser exception {0}", exception.Message));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = exception.Message });
            }
            catch (UserRegistrationException exception)
            {
                _logger.LogError(exception, string.Format("RegisterUser exception {0}", exception.Message));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = exception.Message });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, string.Format("RegisterUser exception {0}", exception.Message));
                return BadRequest(new APIResponse<string> { Error = true, ResponseObject = ResponseLang.Genericexception() });
            }
        }
    }
}
