using MongoStack.ServiceInterface.Interfaces;
using System;
using MongoStack.Core;
using MongoStack.Core.Entities;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace MongoStack.ServiceInterface.Services
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly IRepository<Subcategory, string> subcategoryRepository;
        public SubcategoryService(IRepository<Subcategory, string> subcategoryRepository)
        {
            this.subcategoryRepository = subcategoryRepository;
        }

        public ResultBase CreateSubcategory(Subcategory item)
        {
            throw new NotImplementedException();
        }

        public ResultWithEntities<Subcategory> GetAllSubcategories()
        {
            var result = subcategoryRepository.GetAll();
            return result;
        }

        public ResultWithEntity<Subcategory> GetSubcategoryById(string id)
        {
            var result = subcategoryRepository.GetOne(id);
            return result;
        }

        public ResultBase UpdateSubcategory(Subcategory Subcategory)
        {
            var result = subcategoryRepository.ReplaceOne(Subcategory);
            return result;
        }

        public ResultWithEntities<Subcategory> GetSubcategoriesByExpression(Expression<Func<Subcategory, bool>> predicate)
        {
            var result = subcategoryRepository.GetMany(predicate);
            return result;
        }

        public ResultWithEntity<Subcategory> GetSubcategoryByExpression(Expression<Func<Subcategory, bool>> predicate)
        {
            var result = subcategoryRepository.GetOne(predicate);
            return result;
        }

        public ResultBase DeleteSubcategory(string id)
        {
            var result = subcategoryRepository.DeleteOne(id);
            return result;
        }

        public ResultWithEntities<Subcategory> GetSubcategoriesByFilter(FilterDefinition<Subcategory> filter)
        {
            var result = subcategoryRepository.GetMany(filter);
            return result;
        }
    }
}
