using BrainBox.Data.DTOs.Auth;
using BrainBox.Data.Models;
using System.Linq.Expressions;

namespace BrainBox.Repositories.Contracts
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        /// <summary>
        /// Get refresh token detail
        /// </summary>
        /// <returns></returns>
        Task<RefreshTokenDTO?> GetAsync(Expression<Func<RefreshToken, bool>> predicate);
    }
}
