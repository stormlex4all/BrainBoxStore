using BrainBox.Core.CoreServices.Contracts;
using BrainBox.Core.Exceptions;
using BrainBox.Core.Lang;
using BrainBox.Data.Models;
using BrainBox.Repositories.Contracts;
using System.Linq.Expressions;

namespace BrainBox.Core.CoreServices
{
    public class CoreValidationService<T> : ICoreValidationService<T> where T : BaseModel
    {
        private readonly IRepository<T> _repository;
        public CoreValidationService(IRepository<T> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Validate that no record satisfying the predicate exists in DB
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="SimilarRecordExistsException"></exception>
        public async Task ValidateRecordWithConditionDoesNotExist(Expression<Func<T, bool>> predicate, bool silent)
        {
            if (await _repository.CountAsync(predicate) > 0 && !silent)
            {
                throw new SimilarRecordExistsException(Lang.ResponseLang.SimilarRecordExists(typeof(T).Name));
            }
        }
    }
}
