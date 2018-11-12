using MongoDB.Driver;
using MongoStack.Core;
using MongoStack.Core.DTOs;
using MongoStack.Core.Entities;
using System;
using System.Linq.Expressions;

namespace MongoStack.ServiceInterface.Interfaces
{
    public interface IProductService
    {
        ResultBase UpdateProduct(UpdateProduct category);
        ResultWithEntities<Product> GetAllProducts();
        ResultWithEntity<Product> GetProductById(string id);
        ResultWithEntity<Product> CreateProduct(AddProduct item);
        ResultBase DeleteProduct(string id);
        ResultWithEntities<Product> GetProductsByExpression(Expression<Func<Product, bool>> predicate);
        ResultWithEntities<Product> GetProductsByFilter(FilterDefinition<Product> filter);
    }
}
