using BrainBox.Web.Controllers.Handlers.Contracts;
using BrainBox.Core.Configuration;
using BrainBox.Core.Exceptions;
using BrainBox.Core.Lang;
using BrainBox.Core.Utilities;
using BrainBox.Data.DTOs.Auth;
using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Enums;
using BrainBox.Data.Models;
using BrainBox.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using POSSAP.Core.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BrainBox.Data.DTOs;

namespace BrainBox.Web.Controllers.Handlers
{
    public class AuthenticationHandler : IAuthenticationHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtSettings _jwtSettings;
        private readonly ILogger<AuthenticationHandler> _logger;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly IAPISettings _apiSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly UserManager<BrainBoxUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICartHandler _cartHandler;
        private readonly ICurrentActiveToken _currentActiveToken;


        public AuthenticationHandler(IConfiguration configuration, IJwtSettings jwtSettings, 
                                    ILogger<AuthenticationHandler> logger, IRefreshTokenRepository refreshTokenRepo,
                                    IAPISettings apiSettings, TokenValidationParameters tokenValidationParameters,
                                    UserManager<BrainBoxUser> userManager, RoleManager<IdentityRole> roleManager,
                                    ICartHandler cartHandler, ICurrentActiveToken currentActiveToken)
        {
            _configuration = configuration;
            _jwtSettings = jwtSettings;
            _logger = logger;
            _refreshTokenRepo = refreshTokenRepo;
            _apiSettings = apiSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _userManager = userManager;
            _roleManager = roleManager;
            _cartHandler = cartHandler;
            _currentActiveToken = currentActiveToken;
        }

        /// <summary>
        /// Get accss token on successful validation of user
        /// </summary>
        /// <param name="signInDTO"></param>
        /// <returns>TokenDTO</returns>
        public async Task<TokenDTO> ProcessSignInRequest(SignInDTO signInDTO)
        {
            //user model will be gotten from API calling _membershipService.ValidateUser(email, password);
            return GenerateJwtToken(await ValidateUserForSignIn(signInDTO));
        }

        /// <summary>
        /// Get a new token using refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="user"></param>
        /// <param name="tokenRequestModel"></param>
        /// <returns>TokenDTO</returns>
        public TokenDTO RefreshToken(out RefreshToken refreshToken, out UserDTO user, TokenRequestDTO tokenRequestModel)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            string userEmail = Util.GetTokenUserEmail(tokenRequestModel.Token);

            user = new() { Email = userEmail, Id = Util.GetTokenUserId(tokenRequestModel.Token) };

            //TODO code to get stored refresh token from DB or better still cache using tokenRequestModel.RefreshToken
            refreshToken = GetRefreshToken(userEmail);

            var tokenValidationResult = jwtTokenHandler.ValidateToken(tokenRequestModel.Token, _tokenValidationParameters,
                out var validatedToken);

            return GenerateJwtToken(user, refreshToken.Token);
        }

        /// <summary>
        /// Generates a new access token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public TokenDTO GenerateJwtToken(UserDTO user, string refreshToken = "")
        {
            double defaultTokenExpirePeriod = 1;
            if (_jwtSettings.TokenValidPeriodMinutes != 0)
            {
                defaultTokenExpirePeriod = _jwtSettings.TokenValidPeriodMinutes;
            }
            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(defaultTokenExpirePeriod),
                claims: new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    },
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256)
            );
            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            StoreToken(accessToken);
            if (!string.IsNullOrEmpty(refreshToken))
            {
                return new TokenDTO
                {
                    Token = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = token.ValidTo.AddHours(1)
                };
            }

            return new TokenDTO
            {
                Token = accessToken,
                RefreshToken = GenerateAndSaveRefreshToken(token.Id, user.Email),
                ExpiresAt = token.ValidTo.AddHours(1)
            };
        }

        /// <summary>
        /// Adds a new user to the DB
        /// </summary>
        /// <param name="signUpDTO"></param>
        public async Task<bool> RegisterUser(SignUpDTO signUpDTO)
        {
            if (await _userManager.FindByEmailAsync(signUpDTO.Email) != null)
            {
                throw new UserRegistrationException(ResponseLang.UserAlreadyExists(signUpDTO.Email));
            }

            BrainBoxUser newUser = new()
            {
                Email = signUpDTO.Email,
                UserName = signUpDTO.Username,
                SecurityStamp = new Guid().ToString(),
                UserType = (int)signUpDTO.UserType
            };

            var result = await _userManager.CreateAsync(newUser, signUpDTO.Password);
            if (result.Succeeded)
            {
                await _cartHandler.CreateAsync(new CartDTO { User = new UserDTO { Id = (await _userManager.FindByEmailAsync(newUser.Email)).Id } });
            }
            if (result.Errors != null && result.Errors.Count() > 0)
            {
                throw new UserRegistrationException(result.Errors.First().Description);
            }
            return result.Succeeded;
        }

        /// <summary>
        /// Generates a new refresh token
        /// </summary>
        /// <param name="jwtId"></param>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        private string GenerateAndSaveRefreshToken(string jwtId, string userEmail)
        {
            var rToken = GetRefreshToken(userEmail, false);
            if (rToken != null)
            {
                if (DateTime.Now > rToken.ExpireAt)
                {
                    rToken.IsRevoked = true;
                    rToken.UpdatedAt = DateTime.Now;
                    _refreshTokenRepo.Update(rToken);
                }
                else
                {
                    return rToken.Token;
                }
            }

            var refreshToken = new RefreshToken
            {
                JwtId = jwtId,
                ExpireAt = DateTime.Now.AddMonths(_apiSettings.RefreshTokenValidPeriodMonths),
                IsRevoked = false,
                UserEmail = userEmail,
                Token = Util.Encrypt(userEmail, _apiSettings.AESEncryptionSecret),//to be replaced with a better approach
                CreatedAt = DateTime.Now,
                Id = Guid.NewGuid().ToString()
            };

            //store refreshtoken in DB
            if (!_refreshTokenRepo.Add(refreshToken))
            {
                //throw CouldNotSaveRecordException();
            }

            return refreshToken.Token;
        }

        private RefreshToken GetRefreshToken(string userEmail, bool isForTokenRefresh = true)
        {
            string service = $"{nameof(CacheTenant.Authentication)}";
            var rToken = ObjectCacheProvider.GetCachedObject<RefreshToken>(service, $"{nameof(CachePrefix.RefreshToken)}{userEmail}");
            if (rToken == null)
            {
                rToken = _refreshTokenRepo.Get(r => r.UserEmail == userEmail && !r.IsRevoked).FirstOrDefault();
                if (rToken != null)
                {
                    ObjectCacheProvider.TryCache(service, $"{nameof(CachePrefix.RefreshToken)}{userEmail}", rToken);
                }
            }
            if (rToken == null && isForTokenRefresh)
            {
                throw new Exception($"No Refresh token for email: {userEmail}");
            }
            return rToken;
        }

        private async Task<UserDTO> ValidateUserForSignIn(SignInDTO signInDTO)
        {
            var user = await _userManager.FindByEmailAsync(signInDTO.Email);
            if (user == null)
            {
                throw new UserSignInException(ResponseLang.UserDoesNotExist(signInDTO.Email));
            }
            if (!await _userManager.CheckPasswordAsync(user, signInDTO.Password)) 
            { 
                throw new UserSignInException(ResponseLang.WrongUserCredential());
            }
            return new() { Email = user.Email, UserName = user.UserName, Id = user.Id };
        }

        private void StoreToken(string token)
        {
            _currentActiveToken.Token = token;
        }
    }
}
