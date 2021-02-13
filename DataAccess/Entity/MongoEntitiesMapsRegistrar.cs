using System;
using System.Linq;
using System.Reflection;

namespace DataAccess.Entity
{
    public static class MongoEntitiesMapsRegistrar
    {
        public static void RegisterEntitiesMaps<TMap>()
            where TMap : IMongoEntityMap
        {
            var assembly = Assembly.GetAssembly(typeof(TMap));

            var maps = assembly.GetTypes()
                .Where(t => t.BaseType != null
                            && t.BaseType.IsGenericType
                            && t.BaseType.GetGenericTypeDefinition() == typeof(IMongoEntityMap));

            foreach (var map in maps)
            {
                Activator.CreateInstance(map);
            }
        }

        public static void RegisterEntityMap<TMap>()
            where TMap : IMongoEntityMap
        {
            Activator.CreateInstance(typeof(TMap));
        }
    }
}