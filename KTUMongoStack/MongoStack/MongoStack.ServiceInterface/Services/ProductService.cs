using MongoDB.Driver;
using MongoStack.Core;
using MongoStack.Core.DTOs;
using MongoStack.Core.Entities;
using MongoStack.ServiceInterface.Interfaces;
using System;
using System.Linq.Expressions;

namespace MongoStack.ServiceInterface.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product, string> productRepository;
        public ProductService(IRepository<Product, string> productRepository)
        {
            this.productRepository = productRepository;
        }

        public ResultWithEntity<Product> CreateProduct(AddProduct item)
        {
            var jsonTest = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(jsonTest);

            var result = productRepository.AddOne(obj);
            return result;
        }

        public ResultWithEntities<Product> GetAllProducts()
        {
            var result = productRepository.GetAll();
            return result;
        }

        public ResultWithEntity<Product> GetProductById(string id)
        {
            var result = productRepository.GetOne(id);
            return result;
        }

        public ResultBase UpdateProduct(UpdateProduct item)
        {
            var jsonTest = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(jsonTest);

            var result = productRepository.ReplaceOne(obj);
            return result;
        }

        public ResultWithEntities<Product> GetProductsByExpression(Expression<Func<Product, bool>> predicate)
        {
            var result = productRepository.GetMany(predicate);
            return result;
        }

        public ResultWithEntities<Product> GetProductsByFilter(FilterDefinition<Product> filter)
        {
            var result = productRepository.GetMany(filter);
            return result;
        }

        public ResultBase DeleteProduct(string id)
        {
            var result = productRepository.DeleteOne(id);
            return result;
        }
    }
}
