using System.Collections.Generic;
using MongoDB.Bson;

namespace MongoStack.Core.Entities
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; }
    }

    public static class Validator
    {
        public static bool IsValid(this string idString)
        {
            if(idString.Length != 24 || ObjectId.Parse(idString) == ObjectId.Empty)
            {
                return false;
            }
            return true;
        }
    }
}
