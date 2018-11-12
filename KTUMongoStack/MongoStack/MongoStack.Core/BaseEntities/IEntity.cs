using System;

namespace MongoStack.Core
{
    public interface IEntity { }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
        DateTime CreatedOn { get; set; }
    }
}
