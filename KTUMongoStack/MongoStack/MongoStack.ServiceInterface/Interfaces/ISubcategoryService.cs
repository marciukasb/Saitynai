using MongoDB.Driver;
using MongoStack.Core;
using MongoStack.Core.Entities;
using System;
using System.Linq.Expressions;

namespace MongoStack.ServiceInterface.Interfaces
{
    public interface ISubcategoryService
    {
        ResultBase UpdateSubcategory(Subcategory category);
        ResultWithEntities<Subcategory> GetAllSubcategories();
        ResultWithEntity<Subcategory> GetSubcategoryById(string id);
        ResultBase CreateSubcategory(Subcategory item);
        ResultBase DeleteSubcategory(string id);
        ResultWithEntities<Subcategory> GetSubcategoriesByExpression(Expression<Func<Subcategory, bool>> predicate);
        ResultWithEntity<Subcategory> GetSubcategoryByExpression(Expression<Func<Subcategory, bool>> predicate);
        ResultWithEntities<Subcategory> GetSubcategoriesByFilter(FilterDefinition<Subcategory> filter);
    }
}
