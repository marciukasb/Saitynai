using MongoDB.Driver;
using MongoStack.Core;
using MongoStack.Core.DTOs;
using MongoStack.Core.Entities;
using System;
using System.Linq.Expressions;

namespace MongoStack.ServiceInterface.Interfaces
{
    public interface ICategoryService
    {
        ResultBase UpdateCategory(Category category);
        ResultWithEntities<Category> GetAllCategories();
        ResultWithEntity<Category> GetCategoryById(string id);
        ResultWithEntity<Category> CreateCategory(AddCategory item);
        ResultBase DeleteCategory(string id);
        ResultWithEntities<Category> GetCategoriesByExpression(Expression<Func<Category, bool>> predicate);
        ResultWithEntity<Category> GetCategoryByExpression(Expression<Func<Category, bool>> predicate);
        ResultWithEntities<Category> GetCategoriesByFilter(FilterDefinition<Category> filter);
    }
}
