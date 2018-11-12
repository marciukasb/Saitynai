using System;
using System.Collections.Generic;

namespace MongoStack.Core
{
    public class ResultBase
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public ResultBase()
        {
            Success = false;
            Message = "";
            Exception = null;
        }
    }

    public class ResultWithEntity<T> : ResultBase where T : IEntity
    {
        public T Entity { get; set; }
    }

    public class ResultWithEntities<T> : ResultBase where T : IEntity
    {
        public IEnumerable<T> Entities { get; set; }
    }

    public class ListWithEntities<T> : ResultBase where T : IEntity
    {
        public List<T> Entities { get; set; }
    }
}