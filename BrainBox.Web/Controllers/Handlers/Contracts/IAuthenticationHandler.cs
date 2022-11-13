using BrainBox.Data.DTOs.Auth;
using BrainBox.Data.Models;

namespace BrainBox.Web.Controllers.Handlers.Contracts
{
    public interface IAuthenticationHandler
    {
        /// <summary>
        /// Get accss token on successful validation of user
        /// </summary>
        /// <param name="signInDTO"></param>
        /// <returns>TokenDTO</returns>
        Task<TokenDTO> ProcessSignInRequest(SignInDTO signInDTO);

        /// <summary>
        /// Generates a new access token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        TokenDTO GenerateJwtToken(UserDTO user, string refreshToken = "");

        /// <summary>
        /// Get a new token using refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="user"></param>
        /// <param name="tokenRequestModel"></param>
        /// <returns>TokenDTO</returns>
        TokenDTO RefreshToken(out RefreshToken refreshToken, out UserDTO user, TokenRequestDTO tokenRequestModel);

        /// <summary>
        /// Adds a new user to the DB
        /// </summary>
        /// <param name="signUpDTO"></param>
        Task<bool> RegisterUser(SignUpDTO signUpDTO);
    }
}
