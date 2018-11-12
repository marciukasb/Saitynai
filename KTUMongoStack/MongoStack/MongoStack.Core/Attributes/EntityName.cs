using System;

namespace MongoStack.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class EntityName : Attribute
    {
        public EntityName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Empty attribute entity name not allowed");
            }
            Name = value;
        }

        public string Name { get; set; }
    }
}
