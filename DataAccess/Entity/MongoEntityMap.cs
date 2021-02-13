using MongoDB.Bson.Serialization;

namespace DataAccess.Entity
{
    /// <summary>
    /// Map to Mongo entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public abstract class MongoEntityMap<TEntity> : IMongoEntityMap
    {
        protected MongoEntityMap()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(TEntity)))
            {
                BsonClassMap.RegisterClassMap<TEntity>(Map);
            }
        }

        public abstract void Map(BsonClassMap<TEntity> map);
    }
}