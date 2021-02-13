using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MongoDB.Driver;
using Specification;

namespace DataAccess.Repository
{
    /// <summary>
    /// Base Mongo repository.
    /// </summary>
    /// <typeparam name="TEntity">Entity Type.</typeparam>
    public abstract class MongoRepositoryBase<TEntity> : IRepository<TEntity>
    {
        protected MongoRepositoryBase(IMongoDatabase database)
        {
            Collection = database.GetCollection<TEntity>(CollectionName);
        }

        protected IMongoCollection<TEntity> Collection { get; }

        protected abstract string CollectionName { get; }

        public async IAsyncEnumerable<IEnumerable<TEntity>> GetAll()
        {
            var cursor = await Collection.FindAsync(FilterDefinition<TEntity>.Empty);
            while (await cursor.MoveNextAsync())
            {
                yield return cursor.Current;
            }
        }

        public async IAsyncEnumerable<IEnumerable<TEntity>> Find(SpecificationBase<TEntity> specification)
        {
            var cursor = await Collection.FindAsync(specification.ToExpression());
            while (await cursor.MoveNextAsync())
            {
                yield return cursor.Current;
            }
        }

        public async Task<Maybe<TEntity>> SingleOrNothing(SpecificationBase<TEntity> specification)
        {
            var cursor = await Collection.FindAsync(specification.ToExpression());
            var result = await cursor.SingleOrDefaultAsync();
            return Maybe<TEntity>.From(result);
        }

        public Task Insert(TEntity entity)
        {
            return Collection.InsertOneAsync(entity);
        }

        public Task Delete(SpecificationBase<TEntity> specification)
        {
            return Collection.DeleteOneAsync(specification.ToExpression());
        }
    }
}