using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoStack.Core;
using MongoStack.ServiceInterface.Interfaces;

namespace MongoStack.Data
{
    public class MongoDbRepository<T, TKey> : IRepository<T, TKey> where T : IEntity<TKey>
    {
        private readonly MongoDbContext context;
        private IMongoCollection<T> collection;
        private static int PageSize = 20;

        public MongoDbRepository()
        {
            this.context = MongoDbContext.Context;
            this.collection = GetCollection();
        }

        private IMongoCollection<T> GetCollection()
        {
            return context.GetCollection<T, TKey>();
        }

        public virtual ResultWithEntity<T> GetOne(T entity)
        {
            return GetOne(entity.Id);
        }

        public virtual ResultWithEntity<T> GetOne(TKey id)
        {
            var result = new ResultWithEntity<T>();
            try
            {
                var filter = Builders<T>.Filter.Eq(m => m.Id, id);
                var entity = collection.Find(filter).SingleOrDefault();
                if (entity != null)
                {
                    result.Entity = entity;
                    result.Success = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public virtual ResultWithEntity<T> GetOne(Expression<Func<T, bool>> predicate)
        {
            var result = new ResultWithEntity<T>();
            try
            {
                var entity = collection.Find(predicate).SingleOrDefault();
                if (entity != null)
                {
                    result.Entity = entity;
                    result.Success = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }        

        public virtual ResultWithEntities<T> GetMany(IEnumerable<T> entities)
        {
            return GetMany(entities.Select(entity => entity.Id).ToList());
        }

        public virtual ResultWithEntities<T> GetMany(IEnumerable<TKey> ids)
        {
            var objectids = new List<ObjectId>();
            foreach (var id in ids)
            {
                objectids.Add(new ObjectId(id as string));
            }
            var result = new ResultWithEntities<T>();
            try
            {
                var filter = Builders<T>.Filter.In("_id", objectids);
                var entities = collection.Find(filter).ToList();
                if (entities != null)
                {
                    result.Entities = entities;
                    result.Success = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public ResultWithEntities<T> GetMany(FilterDefinition<T> filter, int? page, int pageSize, out long pages)
        {
            var result = new ResultWithEntities<T>();
            pages = 0;
            try
            {
                var entities = collection.Find(filter).Skip((page - 1) * pageSize).Limit(pageSize).ToList();
                pages = (int)Math.Ceiling((decimal)collection.Find(filter).Count() / pageSize);
                //pages = collection.Find(filter).Count() / pageSize;
                if (entities != null)
                {
                    result.Entities = entities;
                    result.Success = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public ResultWithEntities<T> GetMany(FilterDefinition<T> filter)
        {
            var result = new ResultWithEntities<T>();
            try
            {
                var entities = collection.Find(filter).ToList();
                if (entities != null)
                {
                    result.Entities = entities;
                    result.Success = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }



        // Ids list doesn't work with predicate
        // change did=
        //public virtual ResultWithEntities<T> GetMany(string filterName, List<TKey> values)
        //{
        //    var objectIds = new List<ObjectId>();
        //    values.ForEach(id => objectIds.Add(new ObjectId(id as string)));
        //    var filter = Builders<T>.Filter.In(filterName, objectIds);
        //    return GetMany(filter);
        //}

        public virtual ResultWithEntities<T> GetMany(Expression<Func<T, bool>> predicate, int? page, int pageSize, out long pages)
        {
            var result = new ResultWithEntities<T>();

            pages = 0;
            try
            {
                var entities = collection.Find(predicate).Skip((page - 1) * pageSize).Limit(pageSize).ToList();
                pages = collection.Find(predicate).Count() / pageSize;
                if (entities != null)
                {
                    result.Entities = entities;
                }
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public virtual ResultWithEntities<T> GetMany(Expression<Func<T, bool>> predicate)
        {
            var result = new ResultWithEntities<T>();
            try
            {
                var entities = collection.Find(predicate).ToList();  
                if (entities != null)
                {
                    result.Entities = entities;
                }
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public virtual ResultWithEntities<T> GetAll(int? page, int pageSize, out long pages)
        {
            var result = new ResultWithEntities<T>();
            pages = 0;
            try
            {
                var entities = collection.Find(new BsonDocument()).Skip((page-1) * pageSize).Limit(pageSize).ToList();
                pages = collection.Find(new BsonDocument()).Count() / pageSize;
                if (entities != null)
                {
                    result.Entities = entities;
                }
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public virtual ResultWithEntities<T> GetAll()
        {
            var result = new ResultWithEntities<T>();
            try
            {
                var entities = collection.Find(x => x.Id != null).ToList();
                if (entities != null)
                {
                    result.Entities = entities;
                }
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            var cursor = collection.Find(predicate);
            var count = cursor.Count();
            return (count > 0);
        }

        public virtual long Count()
        {
            return collection.Count(new BsonDocument());
        }

        public virtual long PageCount()
        {
            return Count() / PageSize;
        }

        public virtual long Count(Expression<Func<T, bool>> predicate)
        {
            return collection.Count(predicate);
        }

        public virtual ResultWithEntity<T> AddOne(T entity)
        {
            var result = new ResultWithEntity<T>();
            try
            {
                entity.CreatedOn = DateTime.UtcNow.AddHours(3);
                collection.InsertOne(entity);
                result.Entity = entity;
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Exception = ex;
                return result;
            }
        }

        public virtual ResultBase AddMany(IEnumerable<T> entities)
        {
            var result = new ResultBase();
            try
            {
                collection.InsertMany(entities);
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public virtual ResultBase DeleteOne(T entity)
        {
            return DeleteOne(entity.Id);
        }

        public virtual ResultBase DeleteOne(TKey id)
        {
            var result = new ResultBase();
            try
            {
                var filter = new FilterDefinitionBuilder<T>().Eq(m => m.Id, id);
                var deleteRes = collection.DeleteOne(filter);
                result.Success = true;
                if(deleteRes.DeletedCount == 0) result.Success = false;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public ResultBase DeleteOne(Expression<Func<T, bool>> predicate)
        {
            var result = new ResultBase();
            try
            {
                var deleteRes = collection.DeleteOne(predicate);
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public virtual ResultBase DeleteMany(IEnumerable<T> entities)
        {
            return DeleteMany(entities.Select(entity => entity.Id));
        }

        public virtual ResultBase DeleteMany(IEnumerable<TKey> ids)
        {
            var result = new ResultBase();
            try
            {
                var filter = new FilterDefinitionBuilder<T>().In("_id", ids.Select(id => new ObjectId(id as string)).ToList());
                var deleteRes = collection.DeleteMany(filter);
                if (deleteRes.DeletedCount < 1)
                {
                    result.Message = "Failed to delete document";
                    return result;
                }
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public virtual ResultBase DeleteMany(Expression<Func<T, bool>> predicate)
        {
            var result = new ResultBase();
            try
            {
                var deleteRes = collection.DeleteMany(predicate);
                if (deleteRes.DeletedCount < 1)
                {
                    result.Message = "Failed to delete document";
                    return result;
                }
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public virtual ResultBase ReplaceOne(T entity)
        {
            var result = new ResultBase();
            try
            {
                var filter = new FilterDefinitionBuilder<T>().Eq(m => m.Id, entity.Id);
                var updateRes = collection.ReplaceOne(filter, entity);
                if (updateRes.MatchedCount < 1)
                {
                    return result;
                }
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }
    }
}