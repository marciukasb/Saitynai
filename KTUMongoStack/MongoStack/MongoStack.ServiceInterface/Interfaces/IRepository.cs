using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoStack.Core;

namespace MongoStack.ServiceInterface.Interfaces
{
    public interface IRepository<T, TKey> where T : IEntity<TKey>
    {
        ResultWithEntity<T> GetOne(T entity);
        ResultWithEntity<T> GetOne(TKey id);
        ResultWithEntity<T> GetOne(Expression<Func<T, bool>> predicate);
        ResultWithEntities<T> GetMany(IEnumerable<T> entities);
        ResultWithEntities<T> GetMany(IEnumerable<TKey> ids);

        ResultWithEntities<T> GetMany(FilterDefinition<T> filter, int? page, int pageSize, out long pages);

        ResultWithEntities<T> GetMany(FilterDefinition<T> filter);

        ResultWithEntities<T> GetMany(Expression<Func<T, bool>> predicate, int? page, int pageSize, out long pages);

        ResultWithEntities<T> GetMany(Expression<Func<T, bool>> predicate);

        ResultWithEntities<T> GetAll(int? page, int pageSize, out long pages);

        ResultWithEntities<T> GetAll();

        bool Exists(Expression<Func<T, bool>> predicate);

        long Count();

        long PageCount();

        ResultWithEntity<T> AddOne(T entity);
        ResultBase AddMany(IEnumerable<T> entities);
        ResultBase DeleteOne(T entity);
        ResultBase DeleteOne(Expression<Func<T, bool>> predicate);
        ResultBase DeleteOne(TKey id);
        ResultBase DeleteMany(IEnumerable<T> entities);
        ResultBase DeleteMany(IEnumerable<TKey> ids);
        ResultBase DeleteMany(Expression<Func<T, bool>> predicate);
        ResultBase ReplaceOne(T entity);
    }

    public interface IRepository<T> : IRepository<T, string> where T : IEntity<string>
    { }
}