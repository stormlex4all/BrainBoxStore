using BrainBox.Data;
using BrainBox.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using BrainBox.Data.Models;

namespace BrainBox.Repositories
{
    public class BrainBoxUserRepository : IBrainBoxUserRepository
    {
        private readonly AppDbContext _appDBContext;
        protected readonly DbSet<BrainBoxUser> _dbSet;

        public BrainBoxUserRepository(AppDbContext appDBContext)
        {
            _appDBContext = appDBContext;
            _dbSet = _appDBContext.Set<BrainBoxUser>();
        }

        /// <summary>
        /// Gets untracked user Id
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<string> GetUntrackedUserByEmail(string email)
        {
            var user = await _dbSet.Where(r => !r.IsDeleted).FirstOrDefaultAsync(d => d.Email == email);
            if (user != null)
            {
                _appDBContext.Entry(user).State = EntityState.Detached;
            }
            return user.Id;
        }
    }
}
