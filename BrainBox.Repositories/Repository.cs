using BrainBox.Data;
using BrainBox.Data.DTOs;
using BrainBox.Data.Models;
using BrainBox.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;

namespace BrainBox.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly AppDbContext _appDBContext;
        protected readonly DbSet<T> _dbSet;
        private readonly ICurrentActiveToken _currentActiveToken;
        public Repository(AppDbContext appDBContext, ICurrentActiveToken currentActiveToken)
        {
            _appDBContext = appDBContext;
            _dbSet = _appDBContext.Set<T>();
            _currentActiveToken = currentActiveToken;
        }


        /// <summary>
        /// Get the entity with the given Id
        /// </summary>
        /// <param name="id">Int64</param>
        /// <returns>TEntity</returns>
        public async Task<T?> GetAsync(string id)
        {
            return await _dbSet.Where(r => !r.IsDeleted).FirstOrDefaultAsync(d => d.Id == id);
        }

        /// <summary>
        /// Adds the entity to the DB 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(T entity)
        {
            entity.CreatedAt = DateTime.Now;
            if (string.IsNullOrEmpty(entity.CreatedBy))
            {
                entity.CreatedBy = GetTokenUserId();
            }
            await _dbSet.AddAsync(entity);
            return await _appDBContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Updates the entity in the DB (async)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = GetTokenUserId();
            _dbSet.Update(entity);
            return await _appDBContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Updates the entity in the DB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = GetTokenUserId();
            _dbSet.Update(entity);
            return _appDBContext.SaveChanges() > 0;
        }

        /// <summary>
        /// Adds the entity to the DB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Add(T entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.CreatedBy = GetTokenUserId();
            _dbSet.Add(entity);
            return _appDBContext.SaveChanges() > 0;
        }

        /// <summary>
        /// Get all the records of type T as a list according to predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(r => !r.IsDeleted).Where(predicate).ToList();
        }

        /// <summary>
        /// Get first record of type T according to predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(r => !r.IsDeleted).Where(predicate).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Update multiple numbers of type T in the DB
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(IEnumerable<T> entities)
        {
            using var _transaction = await _appDBContext.Database.BeginTransactionAsync();
            bool saved = false;
            try
            {
                string userId = GetTokenUserId();
                foreach (T record in entities)
                {
                    record.UpdatedAt = DateTime.Now;
                    record.UpdatedBy = userId;
                }
                _dbSet.UpdateRange(entities);
                saved = await _appDBContext.SaveChangesAsync() > 0;
                await _transaction.CommitAsync();
                return saved;
            }
            catch (Exception)
            {
                await _transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Get all the records of type T as a list
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetCollectionAsync() 
        {
            return await _dbSet.Where(r => !r.IsDeleted).ToListAsync();
        }

        /// <summary>
        /// Get all the records of type T as a list according to predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(r => !r.IsDeleted).Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Add multiple items of type T to DB
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(IEnumerable<T> entities)
        {
            using var _transaction = await _appDBContext.Database.BeginTransactionAsync();
            bool saved = false;
            try
            {
                string userId = GetTokenUserId();
                foreach (T record in entities)
                {
                    record.CreatedAt = DateTime.Now;
                    record.CreatedBy = userId;
                }
                await _dbSet.AddRangeAsync(entities);
                saved = await _appDBContext.SaveChangesAsync() > 0;
                await _transaction.CommitAsync();
                return saved;
            }
            catch (Exception)
            {
                await _transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Get count of items of type T in DB satisfying a predicate
        /// </summary>
        /// <returns>Int64</returns>
        public async Task<long> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(r => !r.IsDeleted).Where(predicate).LongCountAsync();
        }

        /// <summary>
        /// Delete single record from db
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(T record)
        {
            record.IsDeleted = true;
            record.UpdatedAt = DateTime.Now;
            record.UpdatedBy = GetTokenUserId();
            return await _appDBContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Delete multiple records from db
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public async Task<bool> Delete(IEnumerable<T> records)
        {
            using var _transaction = await _appDBContext.Database.BeginTransactionAsync();
            bool saved = false;
            try
            {
                string userId = GetTokenUserId();
                foreach (T record in records)
                {
                    record.IsDeleted = true;
                    record.UpdatedAt = DateTime.Now;
                    record.UpdatedBy = userId;
                }
                saved = await _appDBContext.SaveChangesAsync() > 0;
                await _transaction.CommitAsync();
                return saved;
            }
            catch (Exception)
            {
                await _transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Get user Id from token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private string GetTokenUserId()
        {
            string token = _currentActiveToken.Token;
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Token was not saved"); 
            }
            return ((JwtSecurityToken)(new JwtSecurityTokenHandler()).ReadToken(token)).Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value;
        }
    }
}
