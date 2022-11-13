using BrainBox.Data.Models;
using System.Linq.Expressions;

namespace BrainBox.Repositories.Contracts
{
    public interface IRepository<T> where T : BaseModel
    {
        /// <summary>
        /// Get the entity with the given Id
        /// </summary>
        /// <param name="id">Int64</param>
        /// <returns>TEntity</returns>
        Task<T?> GetAsync(string id);

        /// <summary>
        /// Adds the entity to the DB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddAsync(T entity);

        /// <summary>
        /// Updates the entity in the DB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// Updates the entity in the DB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        /// Adds the entity to the DB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Add(T entity);

        /// <summary>
        /// Get all the records of type T as a list according to predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<T> Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get first record of type T according to predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Update multiple numbers of type T in the DB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(IEnumerable<T> entity);

        /// <summary>
        /// Get all the records of type T as a list
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetCollectionAsync();

        /// <summary>
        /// Get all the records of type T as a list according to predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Add multiple items of type T to DB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddAsync(IEnumerable<T> entity);

        /// <summary>
        /// Get count of items of type T in DB satisfying a predicate
        /// </summary>
        /// <returns>Int64</returns>
        Task<long> CountAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Delete single record from db
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(T record);

        /// <summary>
        /// Delete multiple records from db
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        Task<bool> Delete(IEnumerable<T> records);
    }
}
