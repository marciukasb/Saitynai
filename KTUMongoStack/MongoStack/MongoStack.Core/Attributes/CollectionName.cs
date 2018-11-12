using System;

namespace MongoStack.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class CollectionName : Attribute
    {
        public CollectionName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Empty attribute collection name not allowed");
            }
            Name = value;
        }

        public string Name { get; set; }
    }
}
