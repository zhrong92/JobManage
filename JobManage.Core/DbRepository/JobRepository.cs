using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JobManage.Core
{
    public class JobRepository
    {
        private readonly MongoDbRepository<Job> _jobRepository;
        public JobRepository(MongoDbRepository<Job> jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<List<Job>> GetListAsync(Expression<Func<Job, bool>> exp = null)
        {
            if (exp == null)
            {
                return await _jobRepository.GetListAsync();
            }
            else
            {
                return await _jobRepository.GetListAsync(exp);
            }
        }

        public async Task<PageList<Job>> GetPagedListAsync(int pageIndex, int pageSize, string searchValue)
        {
            var count = await _jobRepository.CountAsync();
            var list = await _jobRepository.GetPagedListAsync((pageIndex - 1) * pageSize, pageSize);
            var result = new PageList<Job>()
            {
                TotalCount = count,
                Datas = list
            };
            return result;
        }

        public async Task<Job> InsertAsync(Job job)
        {
            return await _jobRepository.InsertAsync(job);
        }

        public async Task<bool> UpdateStatusAsync(string id, int status)
        {
            var builder = Builders<Job>.Filter;
            var filter = builder.Eq("Id", id);
            var update = Builders<Job>.Update.Set("Status", status);

            return await _jobRepository.UpdateAsync(filter, update, true);
        }

        public async Task<bool> UpdateExecuteAsync(string group, string name, DateTime? previousFireTime, DateTime? nextFireTime)
        {
            var builder = Builders<Job>.Filter;
            var filter = builder.Eq("Group", group) & builder.Eq("Name", name);
            var update = Builders<Job>.Update
                .Set("LastOpTime", previousFireTime)
                .Set("NextOpTime", nextFireTime);

            return await _jobRepository.UpdateAsync(filter, update, true);
        }
    }
}
