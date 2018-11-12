using MongoDB.Bson;

namespace MongoStack.Core.Entities
{
    [CollectionName("Categories")]
    public class Category : Entity
    {
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
