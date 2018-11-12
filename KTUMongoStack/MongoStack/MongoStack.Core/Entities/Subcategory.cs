using MongoDB.Bson;

namespace MongoStack.Core.Entities
{
    [CollectionName("SubCategories")]
    public class Subcategory : Entity
    {
        public ObjectId ParentId { get; set; }
        public string ParentName { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
