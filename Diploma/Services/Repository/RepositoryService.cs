using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Diploma.Models;
using SQLite;

namespace Diploma.Services.Repository
{
    public class RepositoryService : IRepositoryService
    {
        private readonly SQLiteAsyncConnection database;

        public RepositoryService()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            database = new SQLiteAsyncConnection(Path.Combine(path, Constants.DATABASE_NAME));

            InitTable<UserModel>();
            InitTable<LessonModel>();
            InitTable<CourseModel>();
        }

        public async Task<int> DeleteAsync<T>(T entity) where T : IDTOModel, new()
        {
            return await database.DeleteAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate = null) where T : IDTOModel, new()
        {
            var table = database.Table<T>();
            List<T> result;
            if (predicate == null)
            {
                result = await table.ToListAsync();
            }
            else
            {
                result = await table.Where(predicate).ToListAsync();
            }
            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : IDTOModel, new()
        {
            var table = database.Table<T>();
            return await table.ToListAsync();
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : IDTOModel, new()
        {
            return await database.FindAsync(predicate);
        }

        public async Task<T> GetByIdAsync<T>(int id) where T : IDTOModel, new()
        {
            return await database.GetAsync<T>(id);
        }

        public void InitTable<T>() where T : IDTOModel, new()
        {
            database.CreateTableAsync<T>();
        }

        public async Task<int> SaveOrUpdateAsync<T>(T entity) where T : IDTOModel, new()
        {
            return await database.InsertOrReplaceAsync(entity);
        }

        public Task SaveOrUpdateRangeAsync<T>(IEnumerable<T> entities) where T : IDTOModel, new()
        {
            return Task.WhenAll(entities.Select(x => database.InsertOrReplaceAsync(x)));
        }

        public async Task<int> UpdateAsync<T>(T entity) where T : IDTOModel, new()
        {
            return await database.UpdateAsync(entity);
        }
    }
}
