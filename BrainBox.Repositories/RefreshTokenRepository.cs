using BrainBox.Data;
using BrainBox.Data.Models;
using BrainBox.Repositories.Contracts;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using BrainBox.Data.DTOs.Auth;
using BrainBox.Data.DTOs;

namespace BrainBox.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {

        public RefreshTokenRepository(AppDbContext appDBContext, ICurrentActiveToken currentActiveToken) : base(appDBContext, currentActiveToken)
        {
        }

        /// <summary>
        /// Get refresh token detail
        /// </summary>
        /// <returns></returns>
        public async Task<RefreshTokenDTO?> GetAsync(Expression<Func<RefreshToken, bool>> predicate)
        {
            return await _dbSet.Where(predicate).Select(x =>
            new RefreshTokenDTO
            {
                Token = x.Token,
                ExpireAt = x.ExpireAt,
                IsRevoked = x.IsRevoked,
            }).FirstOrDefaultAsync();
        }
    }
}
