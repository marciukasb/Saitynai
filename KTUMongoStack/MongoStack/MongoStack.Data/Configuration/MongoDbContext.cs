using MongoDB.Driver;
using MongoStack.Core;
using MongoStack.Data.Helpers;

namespace MongoStack.Data
{
    public class MongoDbContext
    {
        private static IMongoClient client;
        private static IMongoDatabase database;
        private static MongoDbContext instance;

        public MongoDbContext()
        {
            client = new MongoClient(MongoHelper.GetConnectionString());
            database = client.GetDatabase(MongoHelper.GetDatabaseName());
        }

        public static MongoDbContext Context
        {
            get
            {
                if (instance == null)
                {
                    instance = new MongoDbContext();
                }
                return instance;
            }
        }

        public IMongoCollection<T> GetCollection<T, TKey>() where T : IEntity<TKey>
        {
            return database.GetCollection<T>(MongoHelper.GetCollectionName<T, TKey>());
        }
    }
}