using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JobManage.Core
{
    public class MongoDbRepository<T> where T : IMongoDbEntity
    {
        private IMongoCollection<T> _collection;
        private IMongoQueryable<T> _query;

        public MongoDbRepository(IMongoDbContext dbContext)
        {
            _collection = dbContext.Collection<T>();
            _query = dbContext.Queryable<T>();
        }

        public virtual async Task<T> InsertAsync(T t)
        {
            await _collection.InsertOneAsync(t);
            return t;
        }

        public virtual async Task<bool> UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, bool isUpsert = false)
        {
            var result = await _collection.UpdateManyAsync(filter, update, new UpdateOptions { IsUpsert = isUpsert });
            return result.MatchedCount > 0 || result.UpsertedId != null;
        }

        public virtual async Task<List<T>> GetListAsync()
        {
            return await _query.ToListAsync();
        }

        public virtual async Task<List<T>> GetListAsync(Expression<Func<T, bool>> exp = null)
        {
            if (exp == null)
            {
                return await _query.ToListAsync();
            }
            else
            {
                return await _query.Where(exp).ToListAsync();
            }
        }

        public virtual async Task<List<T>> GetPagedListAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> exp = null)
        {
            int skipCount = (pageIndex - 1) * pageSize;
            int takeCount = pageSize;
            if (exp == null)
            {
                return await _query.Skip(skipCount).Take(takeCount).ToListAsync();
            }
            else
            {
                return await _query.Where(exp).Skip(skipCount).Take(takeCount).ToListAsync();
            }
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> exp = null)
        {
            if (exp == null)
            {
                return await _query.CountAsync();
            }
            else
            {
                return await _query.Where(exp).CountAsync();
            }
        }
    }
}
