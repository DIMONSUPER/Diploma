using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Diploma.Models;

namespace Diploma.Services.Repository
{
    public interface IRepositoryService
    {
        Task<IEnumerable<T>> GetAllAsync<T>() where T : IDTOModel, new();

        Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate = null) where T : IDTOModel, new();

        Task<T> GetByIdAsync<T>(int id) where T : IDTOModel, new();

        Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : IDTOModel, new();

        Task<int> SaveOrUpdateAsync<T>(T entity) where T : IDTOModel, new();

        Task SaveOrUpdateRangeAsync<T>(IEnumerable<T> entities) where T : IDTOModel, new();

        Task<int> UpdateAsync<T>(T entity) where T : IDTOModel, new();

        Task<int> DeleteAsync<T>(T entity) where T : IDTOModel, new();

        void InitTable<T>() where T : IDTOModel, new();
    }
}
