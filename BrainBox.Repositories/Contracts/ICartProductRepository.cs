using BrainBox.Data.DTOs.Store;
using BrainBox.Data.Models;
using System.Linq.Expressions;

namespace BrainBox.Repositories.Contracts
{
    public interface ICartProductRepository : IRepository<CartProduct>
    {
        /// <summary>
        /// Get all cart products satisfying the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IList<CartProductDTO>> GetAllAsync(Expression<Func<CartProduct, bool>> predicate);
    }
}
