using BrainBox.Data.Models;
using System.Linq.Expressions;

namespace BrainBox.Core.CoreServices.Contracts
{
    public interface ICoreValidationService<T> where T : BaseModel
    {
        /// <summary>
        /// Validate that no record satisfying the predicate exists in DB
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="SimilarRecordExistsException"></exception>
        Task ValidateRecordWithConditionDoesNotExist(Expression<Func<T, bool>> predicate, bool silent = false);
    }
}
