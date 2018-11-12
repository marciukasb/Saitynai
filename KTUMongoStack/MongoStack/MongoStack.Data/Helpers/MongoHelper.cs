using System;
using System.Configuration;
using MongoStack.Core;
using MongoStack.Data.Configuration;

namespace MongoStack.Data.Helpers
{
    internal static class MongoHelper
    {
        private static MongoStackDataSection config = (MongoStackDataSection)ConfigurationManager.GetSection("MongoStack/MongoStackData");

        public static string GetConnectionString()
        {
            return config.ConnectionUrl;
        }

        public static string GetDatabaseName()
        {
            return config.DatabaseName;
        }

        public static string GetCollectionName<T, TKey>() where T : IEntity<TKey>
        {
            string collectionName;

            var att = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionName));
            if (att != null)
            {
                collectionName = ((CollectionName)att).Name;
            }
            else
            {
                collectionName = typeof(T).Name + "s";
            }

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Collection name cannot be empty");
            }
            return collectionName;
        }        
    }
}
