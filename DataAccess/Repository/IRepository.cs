using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Specification;

namespace DataAccess.Repository
{
    public interface IRepository<TEntity>
    {
        Task Insert(TEntity entity);

        Task Delete(SpecificationBase<TEntity> specification);

        IAsyncEnumerable<IEnumerable<TEntity>> GetAll();

        IAsyncEnumerable<IEnumerable<TEntity>> Find(SpecificationBase<TEntity> specification);

        Task<Maybe<TEntity>> SingleOrNothing(SpecificationBase<TEntity> specification);
    }
}