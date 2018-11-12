using MongoStack.ServiceInterface.Interfaces;
using System;
using MongoStack.Core;
using MongoStack.Core.Entities;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoStack.Core.DTOs;

namespace MongoStack.ServiceInterface.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category, string> categoryRepository;
        public CategoryService(IRepository<Category, string> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }


     
        public ResultWithEntity<Category> CreateCategory(AddCategory item)
        {
            var jsonTest = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Category>(jsonTest);

            var result = categoryRepository.AddOne(obj);
            return result;
        }

        public ResultWithEntities<Category> GetAllCategories()
        {
            var result = categoryRepository.GetAll();
            return result;
        }

        public ResultWithEntity<Category> GetCategoryById(string id)
        {
            var result = categoryRepository.GetOne(id);
            return result;
        }

        public ResultBase UpdateCategory(Category category)
        {
            var result = categoryRepository.ReplaceOne(category);
            return result;
        }

        public ResultWithEntities<Category> GetCategoriesByExpression(Expression<Func<Category, bool>> predicate)
        {
            var result = categoryRepository.GetMany(predicate);
            return result;
        }

        public ResultWithEntity<Category> GetCategoryByExpression(Expression<Func<Category, bool>> predicate)
        {
            var result = categoryRepository.GetOne(predicate);
            return result;
        }

        public ResultBase DeleteCategory(string id)
        {
            var result = categoryRepository.DeleteOne(id);
            return result;
        }

        public ResultWithEntities<Category> GetCategoriesByFilter(FilterDefinition<Category> filter)
        {
            var result = categoryRepository.GetMany(filter);
            return result;
        }
    }
}
